/**
Copyright 2014-2015 Robert McNeel and Associates

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
#include "util_opengl.h"

extern std::vector<CCScene> scenes;
extern std::vector<ccl::DeviceInfo> devices;
extern std::vector<ccl::SessionParams> session_params;

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
	if (!buffers->get_pass_rect(ccl::PassType::PASS_COMBINED, 1.0f, tile.sample, stride, &pixels[0])) {
		return;
	}

	/* Copy pixels to final image buffer. */
	bool firstpass = true;
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
void CCSession::update_render_tile(ccl::RenderTile &tile)
{
	ccl::thread_scoped_lock pixels_lock(pixels_mutex);
	if (size_has_changed()) return;
	copy_pixels_to_ccsession(this, tile);

	ccl::RenderBuffers* buffers = tile.buffers;
	ccl::BufferParams& params = buffers->params;

	int tilex = params.full_x - session->tile_manager.params.full_x;
	int tiley = params.full_y - session->tile_manager.params.full_y;

	if (update_cbs[this->id] != nullptr) {
		update_cbs[this->id](this->id, tilex, tiley, params.width, params.height, 4, tile.start_sample, tile.num_samples, tile.sample, tile.resolution);
	}
}

/* Wrapper callback for render tile write. Copies tile result into session full image buffer. */
void CCSession::write_render_tile(ccl::RenderTile &tile)
{
	ccl::thread_scoped_lock pixels_lock(pixels_mutex);
	if (size_has_changed()) return;
	copy_pixels_to_ccsession(this, tile);

	ccl::RenderBuffers* buffers = tile.buffers;
	ccl::BufferParams& params = buffers->params;

	auto tilex = params.full_x - session->tile_manager.params.full_x;
	auto tiley = params.full_y - session->tile_manager.params.full_y;
	if (write_cbs[this->id] != nullptr) {
		write_cbs[this->id](this->id, tilex, tiley, params.width, params.height, 4, tile.start_sample, tile.num_samples, tile.sample, tile.resolution);
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

unsigned int cycles_session_create(unsigned int client_id, unsigned int session_params_id, unsigned int scene_id)
{
	ccl::SessionParams params;
	if (session_params_id >= 0 && session_params_id < session_params.size()) {
		params = session_params[session_params_id];
	}

	CCScene& sce = scenes[scene_id];

	int csesid{ -1 };
	int hid{ 0 };

	CCSession* session = CCSession::create(sce.scene->camera->width, sce.scene->camera->height, 4);
	// TODO: pass ccl::Session into CCSession::create
	session->session = new ccl::Session(params);
	session->session->scene = sce.scene;

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

	logger.logit(client_id, "Created session ", session->id, " for scene ", scene_id, " with session_params ", session_params_id);

	return session->id;
}

void cycles_session_destroy(unsigned int client_id, unsigned int session_id)
{
	SESSION_FIND(session_id)

	CCSession* ccses = sessions[session_id];

	for (CCScene& csc : scenes) {
		if (csc.scene == session->scene) {
			csc.scene = nullptr; /* don't delete here, since session deconstructor takes care of it. */
		}
	}

	delete ccsess->session;
	delete ccses;

	sessions[session_id] = nullptr;

	SESSION_FIND_END()
}

void cycles_session_reset(unsigned int client_id, unsigned int session_id, unsigned int width, unsigned int height, unsigned int samples)
{
	SESSION_FIND(session_id)
		logger.logit(client_id, "Reset session ", session_id, ". width ", width, " height ", height, " samples ", samples);
		ccl::thread_scoped_lock pixels_lock(ccsess->pixels_mutex);
		ccsess->reset(width, height, 4);
		ccl::BufferParams bufParams;
		bufParams.width = bufParams.full_width = width;
		bufParams.height = bufParams.full_height = height;
		session->reset(bufParams, (int)samples);
	SESSION_FIND_END()
}

void cycles_session_set_update_callback(unsigned int client_id, unsigned int session_id, void(*update)(unsigned int sid))
{
	SESSION_FIND(session_id)
		CCSession* se = sessions[session_id];
		status_cbs[session_id] = update;
		if (update != nullptr) {
			session->progress.set_update_callback(function_bind<void>(&CCSession::status_update, se));
		}
		else {
			session->progress.set_update_callback(nullptr);
		}
		logger.logit(client_id, "Set status update callback for session ", session_id);
	SESSION_FIND_END()
}

void cycles_session_set_cancel_callback(unsigned int client_id, unsigned int session_id, void(*cancel)(unsigned int sid))
{
	SESSION_FIND(session_id)
		CCSession* se = sessions[session_id];
		cancel_cbs[session_id] = cancel;
		if (cancel != nullptr) {
			session->progress.set_cancel_callback(function_bind<void>(&CCSession::test_cancel, se));
		}
		else {
			session->progress.set_cancel_callback(nullptr);
		}
		logger.logit(client_id, "Set status cancel callback for session ", session_id);
	SESSION_FIND_END()
}

void cycles_session_set_update_tile_callback(unsigned int client_id, unsigned int session_id, RENDER_TILE_CB update_tile_cb)
{
	SESSION_FIND(session_id)
		CCSession* se = sessions[session_id];
		update_cbs[session_id] = update_tile_cb;
		if (update_tile_cb != nullptr) {
			session->update_render_tile_cb = function_bind<void>(&CCSession::update_render_tile, ccsess, std::placeholders::_1);
		}
		else {
			session->update_render_tile_cb = nullptr;
		}
		logger.logit(client_id, "Set render tile update callback for session ", session_id);
	SESSION_FIND_END()
}

void cycles_session_set_write_tile_callback(unsigned int client_id, unsigned int session_id, RENDER_TILE_CB write_tile_cb)
{
	SESSION_FIND(session_id)
		CCSession* se = sessions[session_id];
		write_cbs[session_id] = write_tile_cb;
		if (write_tile_cb != nullptr) {
			session->write_render_tile_cb = function_bind<void>(&CCSession::write_render_tile, ccsess, std::placeholders::_1);
		}
		else {
			session->write_render_tile_cb = nullptr;
		}
		logger.logit(client_id, "Set render tile write callback for session ", session_id);
	SESSION_FIND_END()
}

void cycles_session_set_display_update_callback(unsigned int client_id, unsigned int session_id, DISPLAY_UPDATE_CB display_update_cb)
{
	SESSION_FIND(session_id)
		CCSession* se = sessions[session_id];
		display_update_cbs[session_id] = display_update_cb;
		if (display_update_cb != nullptr) {
			session->display_update_cb = function_bind<void>(&CCSession::display_update, ccsess, std::placeholders::_1);
		}
		else {
			session->display_update_cb = nullptr;
		}
		logger.logit(client_id, "Set display update callback for session ", session_id);
	SESSION_FIND_END()
}

void cycles_session_cancel(unsigned int client_id, unsigned int session_id, const char *cancel_message)
{
	SESSION_FIND(session_id)
		logger.logit(client_id, "Cancel session ", session_id, " with message ", cancel_message);
		session->progress.set_cancel(std::string(cancel_message));
	SESSION_FIND_END()
}

void cycles_session_start(unsigned int client_id, unsigned int session_id)
{
	SESSION_FIND(session_id)
		logger.logit(client_id, "Starting session ", session_id);
		session->start();
	SESSION_FIND_END()
}

void cycles_session_prepare_run(unsigned int client_id, unsigned int session_id)
{
	SESSION_FIND(session_id)
		logger.logit(client_id, "Preparing run for session ", session_id);
		session->prepare_run();
	SESSION_FIND_END()
}

void cycles_session_end_run(unsigned int client_id, unsigned int session_id)
{
	SESSION_FIND(session_id)
		logger.logit(client_id, "Ending run for session ", session_id);
		session->end_run();
	SESSION_FIND_END()
}

bool cycles_session_sample(unsigned int client_id, unsigned int session_id)
{
	bool rc = false;
	SESSION_FIND(session_id)
		logger.logit(client_id, "Starting session ", session_id);
		rc = session->sample();
	SESSION_FIND_END()
	return rc;
}

void cycles_session_wait(unsigned int client_id, unsigned int session_id)
{
	SESSION_FIND(session_id)
		logger.logit(client_id, "Waiting for session ", session_id);
		session->wait();
	SESSION_FIND_END()
}

void cycles_session_set_pause(unsigned int client_id, unsigned int session_id, bool pause)
{
	SESSION_FIND(session_id)
		session->set_pause(pause);
	SESSION_FIND_END()
}

bool cycles_session_is_paused(unsigned int client_id, unsigned int session_id)
{
	SESSION_FIND(session_id)
		return session->is_paused();
	SESSION_FIND_END()

	return false;
}

void cycles_session_set_samples(unsigned int client_id, unsigned int session_id, int samples)
{
	SESSION_FIND(session_id)
		session->set_samples(samples);
	SESSION_FIND_END()
}

void cycles_session_get_buffer_info(unsigned int client_id, unsigned int session_id, unsigned int* buffer_size, unsigned int* buffer_stride)
{
	SESSION_FIND(session_id)
		CCSession* se = sessions[session_id];
		*buffer_size = se->buffer_size;
		*buffer_stride = se->buffer_stride;
		logger.logit(client_id, "Session ", session_id, " get_buffer_info. size ", *buffer_size, " stride ", *buffer_stride);
	SESSION_FIND_END()
}

float* cycles_session_get_buffer(unsigned int client_id, unsigned int session_id)
{
	SESSION_FIND(session_id);
		CCSession* se = sessions[session_id];
		return se->pixels;
	SESSION_FIND_END();

	return nullptr;
}

void cycles_session_copy_buffer(unsigned int client_id, unsigned int session_id, float* pixel_buffer)
{
	SESSION_FIND(session_id)
		CCSession* se = sessions[session_id];
		ccl::thread_scoped_lock pixels_lock(se->pixels_mutex);
		if (se->size_has_changed()) return;
		memcpy(pixel_buffer, se->pixels, se->buffer_size*sizeof(float));
		logger.logit(client_id, "Session ", session_id, " copy complete pixel buffer");
	SESSION_FIND_END()
}

bool initialize_shader_program(GLuint& program)
{
	static const GLchar* vs_src =
#include "vshader.h"

	static const GLchar* fs_src =
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

void cycles_session_rhinodraw(unsigned int client_id, unsigned int session_id, int width, int height, float alpha)
{
	static ccl::DeviceDrawParams draw_params = ccl::DeviceDrawParams();
	draw_params.bind_display_space_shader_cb = nullptr;
	draw_params.unbind_display_space_shader_cb = nullptr;

	SESSION_FIND(session_id)
		ccl::BufferParams session_buf_params;
		session_buf_params.width = session_buf_params.full_width = width;
		session_buf_params.height = session_buf_params.full_height = height;

		if (ccsess->program == 0) {
			if (!initialize_shader_program(ccsess->program)) return;
		}

		draw_params.program = ccsess->program;
		draw_params.alpha = alpha;

		glUseProgram(ccsess->program);
		// let Cycles draw
		session->draw(session_buf_params, draw_params);
		glUseProgram(0);

	SESSION_FIND_END()
}

void cycles_session_draw(unsigned int client_id, unsigned int session_id, int width, int height)
{
	static ccl::DeviceDrawParams draw_params = ccl::DeviceDrawParams();
	draw_params.bind_display_space_shader_cb = nullptr;
	draw_params.unbind_display_space_shader_cb = nullptr;

	SESSION_FIND(session_id)
		ccl::BufferParams session_buf_params;
		session_buf_params.width = session_buf_params.full_width = width;
		session_buf_params.height = session_buf_params.full_height = height;

		session->draw(session_buf_params, draw_params);
	SESSION_FIND_END()
}

void cycles_session_draw_nogl(unsigned int client_id, unsigned int session_id, int width, int height, bool isgpu)
{
	SESSION_FIND(session_id)
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
	SESSION_FIND_END()

}

void cycles_progress_reset(unsigned int client_id, unsigned int session_id)
{
	SESSION_FIND(session_id)
		session->progress.reset();
	SESSION_FIND_END()
}

int cycles_progress_get_sample(unsigned int client_id, unsigned int session_id)
{
	SESSION_FIND(session_id)
		ccl::TileManager &tm = session->tile_manager;
		return tm.state.sample;
	SESSION_FIND_END()

	return INT_MIN;
}

void cycles_progress_get_time(unsigned int client_id, unsigned int session_id, double *total_time, double* sample_time)
{
	SESSION_FIND(session_id)
		return session->progress.get_time(*total_time, *sample_time);
	SESSION_FIND_END()
}

void cycles_tilemanager_get_sample_info(unsigned int client_id, unsigned int session_id, unsigned int* samples, unsigned int* total_samples)
{
	SESSION_FIND(session_id)
		*samples = session->tile_manager.state.sample + 1;
		*total_samples = session->tile_manager.num_samples;
	SESSION_FIND_END()
}

/* Get cycles render progress. Note that progress will be clamped to 1.0f. */
void cycles_progress_get_progress(unsigned int client_id, unsigned int session_id, float* progress)
{
	SESSION_FIND(session_id)
		*progress = session->progress.get_progress();
		if (*progress > 1.0f) *progress = 1.0f;
	SESSION_FIND_END()
}

const char* cycles_progress_get_status(unsigned int client_id, unsigned int session_id)
{
	/* static here, since otherwise std::string goes out of scope on return. */
	static std::string status;
	status = "";
	SESSION_FIND(session_id)
		std::string substatus{ "" };
		session->progress.get_status(status, substatus);
		return status.c_str();
	SESSION_FIND_END()

	return nullptr;
}

const char* cycles_progress_get_substatus(unsigned int client_id, unsigned int session_id)
{
	/* static here, since otherwise std::string goes out of scope on return. */
	static std::string substatus;
	substatus = "";
	SESSION_FIND(session_id)
		std::string status{ "" };
		session->progress.get_status(status, substatus);
		return substatus.c_str();
	SESSION_FIND_END()

	return nullptr;
}