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

// need access to devices
extern std::vector<ccl::DeviceInfo> devices;
extern std::vector<ccl::DeviceInfo> multi_devices;

extern std::vector<ccl::SceneParams> scene_params;
std::vector<CCScene> scenes;

/* Find a ccl::Shader in a given ccl::Scene, based on shader_id
*/
ccl::Shader* find_shader_in_scene(ccl::Scene* sce, unsigned int shader_id)
{
	auto b = sce->shaders.cbegin();
	auto e = sce->shaders.cend();
	ccl::Shader* bg = nullptr;
	unsigned int idx = 0;
	while (b != e) {
		if (idx == shader_id) {
			bg = *b;
			break;
		}
		idx++;
		b++;
	}
	return bg;
}

unsigned int get_idx_for_shader_in_scene(ccl::Scene* sce, ccl::Shader* sh)
{
	auto b = sce->shaders.cbegin();
	auto e = sce->shaders.cend();
	ccl::Shader* bg = nullptr;
	unsigned int idx = 0;
	while (b != e) {
		if (*b == sh) {
			return idx;
		}
		idx++;
		b++;
	}
	return -1;

}

/* implement CCScene methods*/

void CCScene::builtin_image_info(const std::string& builtin_name, void* builtin_data, bool& is_float, int& width, int& height, int& depth, int& channels)
{
	CCImage* img = static_cast<CCImage*>(builtin_data);
	width = img->width;
	height = img->height;
	depth = img->depth;
	channels = img->channels;
	is_float = img->is_float;
}

bool CCScene::builtin_image_pixels(const std::string& builtin_name, void* builtin_data, unsigned char* pixels)
{
	CCImage* img = static_cast<CCImage*>(builtin_data);
	memcpy(pixels, img->builtin_data, img->width*img->height*img->channels*sizeof(unsigned char));
	return false;
}

bool CCScene::builtin_image_float_pixels(const std::string& builtin_name, void* builtin_data, float* pixels)
{
	CCImage* img = static_cast<CCImage*>(builtin_data);
	memcpy(pixels, img->builtin_data, img->width*img->height*img->channels*sizeof(float));
	return false;
}

/* *** */

void _cleanup_scenes()
{
	// clear out scene params vector
	scene_params.clear();

	scenes.clear();
}

unsigned int cycles_scene_create(unsigned int client_id, unsigned int scene_params_id, unsigned int device_id)
{
	CCScene scene;

	ccl::SceneParams params;
	ccl::DeviceInfo di;
	di.num = -1;

	bool found_params{ false };
	bool found_di{ false };

	if (0 <= scene_params_id && scene_params_id < scene_params.size()) {
		params = scene_params[scene_params_id];
		found_params = true;
	}

	GETDEVICE(di, device_id);
	if (di.num >= 0) {
		found_di = true;
	}

	if (found_di && found_params) {
		int cscid{ -1 };
		if (scenes.size() > 0) {
			int hid{ 0 };

			auto scend = scenes.end();
			auto scit = scenes.begin();

			while (scit != scend) {
				if ((*scit).scene == nullptr) {
					cscid = hid;
					break;
				}
				++hid;
				++scit;
			}

		} 
		if (cscid == -1) {
			scenes.push_back(scene);
			cscid = (unsigned int)(scenes.size() - 1);
		}

		scenes[cscid].scene = new ccl::Scene(params, di);
		scenes[cscid].params_id = scene_params_id;
		scenes[cscid].scene->image_manager->builtin_image_info_cb = function_bind(&CCScene::builtin_image_info, scenes[cscid], std::placeholders::_1, std::placeholders::_2, std::placeholders::_3, std::placeholders::_4, std::placeholders::_5, std::placeholders::_6, std::placeholders::_7);
		scenes[cscid].scene->image_manager->builtin_image_pixels_cb = function_bind(&CCScene::builtin_image_pixels, scenes[cscid], std::placeholders::_1, std::placeholders::_2, std::placeholders::_3);
		scenes[cscid].scene->image_manager->builtin_image_float_pixels_cb = function_bind(&CCScene::builtin_image_float_pixels, scenes[cscid], std::placeholders::_1, std::placeholders::_2, std::placeholders::_3);

		logger.logit(client_id, "Created scene ", cscid, " with scene_params ", scene_params_id, " and device ", di.id);
		return cscid;
	}
	else {
		return UINT_MAX;
	}
}

void cycles_scene_set_default_surface_shader(unsigned int client_id, unsigned int scene_id, unsigned int shader_id)
{
	SCENE_FIND(scene_id)
		ccl::Shader* sh = find_shader_in_scene(sce, shader_id);
		sce->default_surface = sh;
		logger.logit(client_id, "Scene ", scene_id, " set default surface shader ", shader_id);
	SCENE_FIND_END()
}

unsigned int cycles_scene_get_default_surface_shader(unsigned int client_id, unsigned int scene_id)
{
	SCENE_FIND(scene_id)
		return get_idx_for_shader_in_scene(sce, sce->default_surface);
	SCENE_FIND_END()

	return UINT_MAX;
}

void cycles_scene_reset(unsigned int client_id, unsigned int scene_id)
{
	SCENE_FIND(scene_id)
		sce->reset();
	SCENE_FIND_END()
}

bool cycles_scene_try_lock(unsigned int client_id, unsigned int scene_id)
{
	SCENE_FIND(scene_id)
		return sce->mutex.try_lock();
	SCENE_FIND_END()
	return false;
}

void cycles_scene_lock(unsigned int client_id, unsigned int scene_id)
{
	SCENE_FIND(scene_id)
		sce->mutex.lock();
	SCENE_FIND_END()
}

void cycles_scene_unlock(unsigned int client_id, unsigned int scene_id)
{
	SCENE_FIND(scene_id)
		sce->mutex.unlock();
	SCENE_FIND_END()
}

