/**
Copyright 2014-2017 Robert McNeel and Associates

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
**/

#include "internal_types.h"
#include "util_thread.h"
#include "util_opengl.h"

/* Hold all created sessions. */
std::vector<CCSession*> sessions;

/* Four vectors to hold registered callback functions.
 * For each created session a corresponding idx into these
 * vectors will exist, but in the case no callback
 * was registered the value for idx will be nullptr.
 */
std::vector<STATUS_UPDATE_CB> status_cbs;
std::vector<TEST_CANCEL_CB> cancel_cbs;
std::vector<RENDER_TILE_CB> update_cbs;
std::vector<RENDER_TILE_CB> write_cbs;
std::vector<DISPLAY_UPDATE_CB> display_update_cbs;

static ccl::thread_mutex session_mutex;

/* Find pointers for CCSession and ccl::Session. Return false if either fails. */
bool session_find(unsigned int sid, CCSession** ccsess, ccl::Session** session)
{
	ccl::thread_scoped_lock lock(session_mutex);
	if (0 <= (sid) && (sid) < sessions.size()) {
		*ccsess = sessions[sid];
		if(*ccsess!=nullptr) *session = (*ccsess)->session;
		return *ccsess!=nullptr && *session!=nullptr;
	}
	return false;
}

/* Wrap status update callback. */
void CCSession::status_update(void) {
	if (status_cbs[this->id] != nullptr) {
		status_cbs[this->id](this->id);
	}
}

/* Wrap status update callback. */
void CCSession::test_cancel(void) {
	if (cancel_cbs[this->id] != nullptr) {
		cancel_cbs[this->id](this->id);
	}
}

/* floats per pixel (rgba). */
const int stride{ 4 };

/* copy the pixel buffer from RenderTile to the final pixel buffer in CCSession. */
void copy_pixels_to_ccsession(CCSession* se, ccl::RenderTile &tile) {

	ccl::RenderBuffers* buffers = tile.buffers;
	/* always do copy_from_device(). This is necessary when rendering is done
	 * on i.e. GPU or network node.
	 */
	buffers->copy_from_device();
	ccl::BufferParams& params = buffers->params;

	/* have a local float buffer to copy tile buffer to. */
	std::vector<float> pixels(params.width*params.height * stride, 0.5f);

	int scewidth = params.full_width;
	int sceheight = params.full_height;

	int tilex = params.full_x - se->session->tile_manager.params.full_x;
	int tiley = params.full_y - se->session->tile_manager.params.full_y;

	/* Copy the tile buffer to pixels. */
	if (!buffers->get_pass_rect("combined", 1.0f, tile.sample, stride, &pixels[0])) {
		return;
	}

	/* Copy pixels to final image buffer. */
	for (int y = 0; y < params.height; y++) {
		for (int x = 0; x < params.width; x++) {
			/* from tile pixels coord. */
			int tileidx = y * params.width * stride + x * stride;
			/* to full image pixels coord. */
			int fullimgidx = (sceheight - (tiley + y) - 1) * scewidth * stride + (tilex + x) * stride;

			/* copy the tile pixels from pixels into session final pixel buffer. */
			se->pixels[fullimgidx + 0] = pixels[tileidx + 0];
			se->pixels[fullimgidx + 1] = pixels[tileidx + 1];
			se->pixels[fullimgidx + 2] = pixels[tileidx + 2];
			se->pixels[fullimgidx + 3] = pixels[tileidx + 3];
		}
	}
}

/* Wrapper callback for render tile update. Copies tile result into session full image buffer. */
void CCSession::update_render_tile(ccl::RenderTile &tile, bool highlight)
{
	/*
	if (update_cbs[this->id] != nullptr) {
		write_or_update_cb(this, tile, update_cbs[this->id]);
	}
	*/
}

/* Wrapper callback for render tile write. Copies tile result into session full image buffer. */
void CCSession::write_render_tile(ccl::RenderTile &tile)
{
	ccl::thread_scoped_lock pixels_lock(pixels_mutex);
	if (size_has_changed()) return;
	//copy_pixels_to_ccsession(session, tile);

	ccl::RenderBuffers* buffers = tile.buffers;
	ccl::BufferParams& params = buffers->params;
	ccl::vector<ccl::Pass>& passes = params.passes;

	auto tilex = params.full_x - session->tile_manager.params.full_x;
	auto tiley = params.full_y - session->tile_manager.params.full_y;

	ccl::vector<float> px(params.width*params.height * 4);
	float exposure = session->scene->film->exposure;
	int sample = tile.sample;

	if (write_cbs[this->id] != nullptr) {
		buffers->copy_from_device();
		for(int i = 0, c = passes.size(); i < c; i++) {
			ccl::Pass p = passes[i];
			int components = p.components;
			ccl::PassType pt = p.type;
			int pixlen = params.width*params.height*components;
			if (buffers->get_pass_rect(p.name, p.exposure ? exposure : 1.0f, sample, components, &px[0])) {
				write_cbs[this->id](id, tilex, tiley, params.width, params.height, sample, components, (int)p.type, &px[0], pixlen);
			}
		}
	}
}

/* Wrapper callback for display update stuff. When this is called one pass has been conducted. */
void CCSession::display_update(int sample)
{
	//ccl::thread_scoped_lock pixels_lock(pixels_mutex);
	if (size_has_changed()) return;
	if (display_update_cbs[this->id] != nullptr) {
		display_update_cbs[this->id](this->id, sample);
	}
}

/**
 * Clean up resources acquired during this run of Cycles.
 */
void _cleanup_sessions()
{
#if 0
	for (CCSession* se : sessions) {
		if (se == nullptr) continue;

		{
			delete[] se->pixels;
			se->pixels = nullptr;
			delete se->session;
			se->session = nullptr;
		}
		delete se;
	}

	sessions.clear();
	session_params.clear();

	status_cbs.clear();
	cancel_cbs.clear();
	update_cbs.clear();
	write_cbs.clear();
	update_cbs.clear();
#endif
}

CCSession* CCSession::create(int width, int height, unsigned int buffer_stride) {
	int img_size{ width * height };
	float* pixels_ = new float[img_size*buffer_stride]{0};
	memset(pixels_, 0, sizeof(float)*img_size*buffer_stride);
	CCSession* se = new CCSession(pixels_, img_size*buffer_stride, buffer_stride);
	se->width = width;
	se->height = height;
	se->_size_has_changed = false;

	return se;
}
void CCSession::reset(int width_, int height_, unsigned int buffer_stride_) {
	int img_size = width_ * height_;
	if (img_size*buffer_stride_ != buffer_size || buffer_stride_ != buffer_stride) {
		delete[] pixels;

		pixels = new float[img_size*buffer_stride_] {0};
		memset(pixels, 0, sizeof(float)*img_size*buffer_stride);
		buffer_size = img_size*buffer_stride_;
		buffer_stride = buffer_stride_;
		width = width_;
		height = height_;
		_size_has_changed = true;
	}
}

bool CCSession::size_has_changed() {
	bool rc = _size_has_changed;
	_size_has_changed = false;
	return rc;
}

unsigned int cycles_session_create(unsigned int client_id, unsigned int session_params_id)
{
	ccl::thread_scoped_lock lock(session_mutex);
	ccl::SessionParams* params = nullptr;
	if (session_params_id < session_params.size()) {
		params = session_params[session_params_id];
	}

	int csesid{ -1 };
	int hid{ 0 };

	CCSession* session = CCSession::create(10, 10, 4);
	session->session = new ccl::Session(*params);

	auto csessit = sessions.begin();
	auto csessend = sessions.end();
	while (csessit != csessend) {
		if ((*csessit) == nullptr) {
			csesid = hid;
		}
		++hid;
		++csessit;
	}

	if (csesid == -1) {
		sessions.push_back(session);
		csesid = (unsigned int)(sessions.size() - 1);
		status_cbs.push_back(nullptr);
		cancel_cbs.push_back(nullptr);
		update_cbs.push_back(nullptr);
		write_cbs.push_back(nullptr);
		display_update_cbs.push_back(nullptr);
	}
	else {
		sessions[csesid] = session;
		status_cbs[csesid] = nullptr;
		update_cbs[csesid] = nullptr;
		write_cbs[csesid] = nullptr;
		display_update_cbs[csesid] = nullptr;
	}


	session->id = csesid;

	logger.logit(client_id, "Created session ", session->id, " with session_params ", session_params_id);

	return session->id;
}

void cycles_session_set_scene(unsigned int client_id, unsigned int session_id, unsigned int scene_id)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		CCScene* csce = nullptr;
		ccl::Scene* sce = nullptr;
		if (scene_find(scene_id, &csce, &sce)) {
			session->scene = sce;
		}
	}
}

void cycles_session_destroy(unsigned int client_id, unsigned int session_id)
{
#if 0
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {

		scene_clear_pointer(session->scene);

		delete ccsess->session;
		delete ccsess;

		sessions[session_id] = nullptr;
	}
#endif
}

static ccl::vector<ccl::Pass> _passes;
ccl::vector<ccl::Pass>& get_passes() {
	ccl::Pass::add(ccl::PASS_COMBINED, _passes, "combined");
	ccl::Pass::add(ccl::PASS_DEPTH, _passes, "depth");
	ccl::Pass::add(ccl::PASS_NORMAL, _passes, "normal");
	/*ccl::Pass::add(ccl::PASS_DIFFUSE_INDIRECT, _passes);
	ccl::Pass::add(ccl::PASS_UV, _passes);
	ccl::Pass::add(ccl::PASS_AO, _passes);
	ccl::Pass::add(ccl::PASS_OBJECT_ID, _passes);
	ccl::Pass::add(ccl::PASS_MATERIAL_ID, _passes);
	ccl::Pass::add(ccl::PASS_DIFFUSE_COLOR, _passes);
	ccl::Pass::add(ccl::PASS_DIFFUSE_DIRECT, _passes);
	ccl::Pass::add(ccl::PASS_DIFFUSE_INDIRECT, _passes);
	ccl::Pass::add(ccl::PASS_GLOSSY_COLOR, _passes);
	ccl::Pass::add(ccl::PASS_GLOSSY_DIRECT, _passes);
	ccl::Pass::add(ccl::PASS_GLOSSY_INDIRECT, _passes);
	ccl::Pass::add(ccl::PASS_EMISSION, _passes);
	ccl::Pass::add(ccl::PASS_TRANSMISSION_COLOR, _passes);
	ccl::Pass::add(ccl::PASS_TRANSMISSION_DIRECT, _passes);
	ccl::Pass::add(ccl::PASS_TRANSMISSION_INDIRECT, _passes);
	ccl::Pass::add(ccl::PASS_SUBSURFACE_COLOR, _passes);
	ccl::Pass::add(ccl::PASS_SUBSURFACE_DIRECT, _passes);
	ccl::Pass::add(ccl::PASS_SUBSURFACE_INDIRECT, _passes);
	ccl::Pass::add(ccl::PASS_SHADOW, _passes);*/
	return _passes;
}


void cycles_session_reset(unsigned int client_id, unsigned int session_id, unsigned int width, unsigned int height, unsigned int samples, unsigned int full_x, unsigned int full_y, unsigned int full_width, unsigned int full_height )
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		logger.logit(client_id, "Reset session ", session_id, ". width ", width, " height ", height, " samples ", samples);
		ccl::thread_scoped_lock pixels_lock(ccsess->pixels_mutex);
		ccsess->reset(width, height, 4);
		ccsess->buffer_params.full_x = full_x;
		ccsess->buffer_params.full_y = full_y;
		ccsess->buffer_params.full_width = full_width;
		ccsess->buffer_params.full_height = full_height;
		ccsess->buffer_params.width = width;
		ccsess->buffer_params.height = height;

		ccl::vector<ccl::Pass>& passes = get_passes();

		session->scene->film->tag_passes_update(session->scene, passes);
		session->scene->film->display_pass = ccl::PassType::PASS_COMBINED;
		session->scene->film->tag_update(session->scene);

		ccsess->buffer_params.passes = passes;

		session->reset(ccsess->buffer_params, (int)samples);
	}
}

void cycles_session_set_update_callback(unsigned int client_id, unsigned int session_id, void(*update)(unsigned int sid))
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		CCSession* se = sessions[session_id];
		status_cbs[session_id] = update;
		if (update != nullptr) {
			session->progress.set_update_callback(function_bind<void>(&CCSession::status_update, se));
		}
		else {
			session->progress.set_update_callback(nullptr);
		}
		logger.logit(client_id, "Set status update callback for session ", session_id);
	}
}

void cycles_session_set_cancel_callback(unsigned int client_id, unsigned int session_id, void(*cancel)(unsigned int sid))
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		CCSession* se = sessions[session_id];
		cancel_cbs[session_id] = cancel;
		if (cancel != nullptr) {
			session->progress.set_cancel_callback(function_bind<void>(&CCSession::test_cancel, se));
		}
		else {
			session->progress.set_cancel_callback(nullptr);
		}
		logger.logit(client_id, "Set status cancel callback for session ", session_id);
	}
}

void cycles_session_set_update_tile_callback(unsigned int client_id, unsigned int session_id, RENDER_TILE_CB update_tile_cb)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		update_cbs[session_id] = update_tile_cb;
		if (update_tile_cb != nullptr) {
			session->update_render_tile_cb = function_bind<void>(&CCSession::update_render_tile, ccsess, std::placeholders::_1, std::placeholders::_2);
		}
		else {
			session->update_render_tile_cb = nullptr;
		}
		logger.logit(client_id, "Set render tile update callback for session ", session_id);
	}
}

void cycles_session_set_write_tile_callback(unsigned int client_id, unsigned int session_id, RENDER_TILE_CB write_tile_cb)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		write_cbs[session_id] = write_tile_cb;
		if (write_tile_cb != nullptr) {
			session->write_render_tile_cb = function_bind<void>(&CCSession::write_render_tile, ccsess, std::placeholders::_1);
		}
		else {
			session->write_render_tile_cb = nullptr;
		}
		logger.logit(client_id, "Set render tile write callback for session ", session_id);
	}
}

void cycles_session_set_display_update_callback(unsigned int client_id, unsigned int session_id, DISPLAY_UPDATE_CB display_update_cb)
{
#if 0
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		CCSession* se = sessions[session_id];
		display_update_cbs[session_id] = display_update_cb;
		if (display_update_cb != nullptr) {
			session->display_update_cb = function_bind<void>(&CCSession::display_update, ccsess, std::placeholders::_1);
		}
		else {
			session->display_update_cb = nullptr;
		}
		logger.logit(client_id, "Set display update callback for session ", session_id);
	}
#endif
}

void cycles_session_cancel(unsigned int client_id, unsigned int session_id, const char *cancel_message)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		logger.logit(client_id, "Cancel session ", session_id, " with message ", cancel_message);
		session->progress.set_cancel(std::string(cancel_message));
	}
}

void cycles_session_start(unsigned int client_id, unsigned int session_id)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		logger.logit(client_id, "Starting session ", session_id);
		session->start();
	}
}

void cycles_session_prepare_run(unsigned int client_id, unsigned int session_id)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		logger.logit(client_id, "Preparing run for session ", session_id);
		session->prepare_run();
	}
}

void cycles_session_end_run(unsigned int client_id, unsigned int session_id)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		logger.logit(client_id, "Ending run for session ", session_id);
		session->end_run();
	}
}

int cycles_session_sample(unsigned int client_id, unsigned int session_id)
{
	int rc = -1;
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		logger.logit(client_id, "Starting session ", session_id);
		rc = session->sample();
	}
	return rc;
}

void cycles_session_wait(unsigned int client_id, unsigned int session_id)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		logger.logit(client_id, "Waiting for session ", session_id);
		session->wait();
	}
}

void cycles_session_set_pause(unsigned int client_id, unsigned int session_id, bool pause)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		session->set_pause(pause);
	}
}

bool cycles_session_is_paused(unsigned int client_id, unsigned int session_id)
{
/*
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		return false; // session->is_paused();
	}*/

	return false;
}

void cycles_session_set_samples(unsigned int client_id, unsigned int session_id, int samples)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		session->set_samples(samples);
	}
}

void cycles_session_get_buffer_info(unsigned int client_id, unsigned int session_id, unsigned int* buffer_size, unsigned int* buffer_stride)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		(void)session;
		*buffer_size = ccsess->buffer_size;
		*buffer_stride = ccsess->buffer_stride;
		logger.logit(client_id, "Session ", session_id, " get_buffer_info. size ", *buffer_size, " stride ", *buffer_stride);
	}
}

float* cycles_session_get_buffer(unsigned int client_id, unsigned int session_id)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		(void)session;
		return ccsess->pixels;
	};

	return nullptr;
}

void cycles_session_copy_buffer(unsigned int client_id, unsigned int session_id, float* pixel_buffer)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		(void)session;
		ccl::thread_scoped_lock pixels_lock(ccsess->pixels_mutex);
		if (ccsess->size_has_changed()) return;
		memcpy(pixel_buffer, ccsess->pixels, ccsess->buffer_size*sizeof(float));
		logger.logit(client_id, "Session ", session_id, " copy complete pixel buffer");
	}
}

bool initialize_shader_program(GLuint& program)
{
#include "vshader.h"
#include "fshader.h"

	if (glewInit() != GLEW_OK) {
		return false;
	}

	GLuint vs = glCreateShader(GL_VERTEX_SHADER);
	glShaderSource(vs, 1, &vs_src, nullptr);
	glCompileShader(vs);

#ifdef _DEBUG
	{
		GLint log_length;
		glGetShaderiv(vs, GL_INFO_LOG_LENGTH, &log_length);

		if (log_length > 0)
		{
			std::string log;
			log.reserve(log_length);

			glGetShaderInfoLog(vs, log_length, nullptr, (GLchar*)log.c_str());
			OutputDebugStringA(log.c_str());
			assert(false);
		}
	}
#endif

	GLint success = GL_FALSE;
	glGetShaderiv(vs, GL_COMPILE_STATUS, &success);

	if (success == GL_FALSE)
	{
		glDeleteShader(vs);
		assert(false);
		return false;
	}

	GLuint fs = glCreateShader(GL_FRAGMENT_SHADER);
	glShaderSource(fs, 1, &fs_src, nullptr);
	glCompileShader(fs);

#ifdef _DEBUG
	{
		GLint log_length;
		glGetShaderiv(fs, GL_INFO_LOG_LENGTH, &log_length);

		if (log_length > 0)
		{
			std::string log;
			log.reserve(log_length);

			glGetShaderInfoLog(fs, log_length, nullptr, (GLchar*)log.c_str());
			OutputDebugStringA(log.c_str());
			assert(false);
		}
	}
#endif

	success = GL_FALSE;
	glGetShaderiv(fs, GL_COMPILE_STATUS, &success);

	if (success == GL_FALSE)
	{
		glDeleteShader(vs);
		glDeleteShader(fs);
		assert(false);
		return false;
	}

	program = glCreateProgram();
	glAttachShader(program, vs);
	glAttachShader(program, fs);
	glLinkProgram(program);

#ifdef _DEBUG
	{
		GLint log_length;
		glGetProgramiv(program, GL_INFO_LOG_LENGTH, &log_length);

		if (log_length > 0)
		{
			std::string log;
			log.reserve(log_length);

			glGetProgramInfoLog(program, log_length, nullptr, (GLchar*)log.c_str());
			OutputDebugStringA(log.c_str());
			assert(false);
		}
	}
#endif

	glDeleteShader(vs);
	glDeleteShader(fs);

	success = GL_FALSE;
	glGetProgramiv(program, GL_LINK_STATUS, &success);

	if (success == GL_FALSE)
	{
		assert(false);
		return false;
	}

	return true;
}

void cycles_session_rhinodraw(unsigned int client_id, unsigned int session_id, float alpha)
{
	ccl::DeviceDrawParams draw_params = ccl::DeviceDrawParams();
	draw_params.bind_display_space_shader_cb = nullptr;
	draw_params.unbind_display_space_shader_cb = nullptr;

	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {

		if (ccsess->program == 0) {
			if (!initialize_shader_program(ccsess->program)) return;
		}

		draw_params.program = ccsess->program;
		draw_params.alpha = alpha;

		glUseProgram(ccsess->program);

		bool depthEnabled = glIsEnabled(GL_DEPTH_TEST);
		if (depthEnabled) {
			glDisable(GL_DEPTH_TEST);
		}

		// let Cycles draw
		session->draw(ccsess->buffer_params, draw_params);
		if (depthEnabled)
		{
			glEnable(GL_DEPTH_TEST);
		}

		glUseProgram(0);

	}
}

void cycles_session_draw(unsigned int client_id, unsigned int session_id)
{
	ccl::DeviceDrawParams draw_params = ccl::DeviceDrawParams();
	draw_params.bind_display_space_shader_cb = nullptr;
	draw_params.unbind_display_space_shader_cb = nullptr;

	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		session->draw(ccsess->buffer_params, draw_params);
	}
}

void cycles_session_draw_nogl(unsigned int client_id, unsigned int session_id, bool isgpu)
{
#if 0
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		// lock moved to initial callback invocation
		//ccl::thread_scoped_lock pixels_lock(ccsess->pixels_mutex);
		if (!ccsess->pixels || ccsess->size_has_changed()) return;
		ccl::BufferParams session_buf_params;
		ccl::DeviceDrawParams draw_params;
		session_buf_params.width = session_buf_params.full_width = width;
		session_buf_params.height = session_buf_params.full_height = height;
		if (isgpu)
			session->draw(session_buf_params, draw_params);
		session->display->get_pixels(session->device, ccsess->pixels);
	}
#endif
}

void cycles_progress_reset(unsigned int client_id, unsigned int session_id)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		session->progress.reset();
	}
}

int cycles_progress_get_sample(unsigned int client_id, unsigned int session_id)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		ccl::TileManager &tm = session->tile_manager;
		return tm.state.sample;
	}

	return INT_MIN;
}

void cycles_progress_get_time(unsigned int client_id, unsigned int session_id, double *total_time, double* sample_time)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		return session->progress.get_time(*total_time, *sample_time);
	}
}

void cycles_tilemanager_get_sample_info(unsigned int client_id, unsigned int session_id, unsigned int* samples, unsigned int* total_samples)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		*samples = session->tile_manager.state.sample + 1;
		*total_samples = session->tile_manager.num_samples;
	}
}

/* Get cycles render progress. Note that progress will be clamped to 1.0f. */
void cycles_progress_get_progress(unsigned int client_id, unsigned int session_id, float* progress)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		*progress = session->progress.get_progress();
		if (*progress > 1.0f) *progress = 1.0f;
	}
}

class StringHolder
{
public:
	std::string thestring;
};

void* cycles_string_holder_new()
{
	return new StringHolder();
}

void cycles_string_holder_delete(void* strholder)
{
	StringHolder* holder = (StringHolder*)strholder;
	delete holder;
	holder = nullptr;
}

const char* cycles_string_holder_get(void* strholder)
{
	StringHolder* holder = (StringHolder*)strholder;
	if(holder!=nullptr) {
		return holder->thestring.c_str();
	}
	return "";
}

bool cycles_progress_get_status(unsigned int client_id, unsigned int session_id, void* strholder)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		StringHolder* holder = (StringHolder*)strholder;
		std::string substatus{ "" };
		session->progress.get_status(holder->thestring, substatus);
		return true;
	}

	return false;
}

bool cycles_progress_get_substatus(unsigned int client_id, unsigned int session_id, void* strholder)
{
	CCSession* ccsess = nullptr;
	ccl::Session* session = nullptr;
	if (session_find(session_id, &ccsess, &session)) {
		StringHolder* holder = (StringHolder*)strholder;
		std::string status{ "" };
		session->progress.get_status(status, holder->thestring);
		return true;
	}

	return false;
}
