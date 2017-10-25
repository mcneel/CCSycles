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

extern std::vector<CCScene> scenes;

std::vector<CCShader*> shaders;

std::vector<CCImage*> images;

void _init_shaders()
{
	cycles_create_shader(0); // default surface
	cycles_create_shader(0); // default light
	cycles_create_shader(0); // default background
	cycles_create_shader(0); // default empty
}

void _cleanup_shaders()
{
	for (CCShader* sh : shaders) {
		// just setting to nullptr, as scene disposal frees this memory.
		if (sh != nullptr) {
			sh->graph = nullptr;
			sh->shader = nullptr;
			sh->scene_mapping.clear();
			delete sh;
		}
	}
	shaders.clear();
}

void _cleanup_images()
{
	for (CCImage* img : images) {
		if (img == nullptr) continue;

		delete[] img->builtin_data;
		img->builtin_data = nullptr;

		delete img;
	}
	images.clear();
}

ccl::ShaderNode* _shader_node_find(unsigned int shader_id, unsigned int shnode_id)
{
	CCShader* sh = shaders[shader_id];
	auto psh = sh->graph->nodes.begin();
	auto end = sh->graph->nodes.end();
	while (psh != end)
	{
		ccl::ShaderNode* shn = (*psh);
		if (shn->id == shnode_id) {
			return shn;
		}
		++psh;
	}
	return nullptr;
}

/* Create a new shader.
 TODO: name for shader
*/
unsigned int cycles_create_shader(unsigned int client_id)
{
	CCShader* sh = new CCShader();
	sh->shader->graph = sh->graph;
	shaders.push_back(sh);

	return (unsigned int)(shaders.size() - 1);
}

/* Add shader to specified scene. */
unsigned int cycles_scene_add_shader(unsigned int client_id, unsigned int scene_id, unsigned int shader_id)
{
	SCENE_FIND(scene_id)
		CCShader* sh = shaders[shader_id];
	sce->shaders.push_back(sh->shader);
	sh->shader->tag_update(sce);
	sh->shader->tag_used(sce);
	unsigned int shid = (unsigned int)(sce->shaders.size() - 1);
	sh->scene_mapping.insert({ scene_id, shid });
	return shid;
	SCENE_FIND_END()

		return (unsigned int)(-1);
}

void cycles_scene_tag_shader(unsigned int client_id, unsigned int scene_id, unsigned int shader_id)
{
	SCENE_FIND(scene_id)
		CCShader* sh = shaders[shader_id];
	sh->shader->tag_update(sce);
	sh->shader->tag_used(sce);
	SCENE_FIND_END()
}

/* Get Cycles shader ID in specific scene. */
unsigned int cycles_scene_shader_id(unsigned int client_id, unsigned int scene_id, unsigned int shader_id)
{
	CCShader* sh = shaders[shader_id];
	if (sh->scene_mapping.find(scene_id) != sh->scene_mapping.end()) {
		return sh->scene_mapping[scene_id];
	}

	return (unsigned int)(-1);
}

void cycles_shader_new_graph(unsigned int client_id, unsigned int shader_id)
{
	CCShader* sh = shaders[shader_id];
	sh->graph = new ccl::ShaderGraph();
	sh->shader->set_graph(sh->graph);
}


void cycles_shader_set_name(unsigned int client_id, unsigned int shader_id, const char* name)
{
	SHADER_SET(shader_id, std::string, name, name);
}

void cycles_shader_set_use_mis(unsigned int client_id, unsigned int shader_id, unsigned int use_mis)
{
	SHADER_SET(shader_id, bool, use_mis, use_mis == 1)
}

void cycles_shader_set_use_transparent_shadow(unsigned int client_id, unsigned int shader_id, unsigned int use_transparent_shadow)
{
	SHADER_SET(shader_id, bool, use_transparent_shadow, use_transparent_shadow == 1)
}

void cycles_shader_set_heterogeneous_volume(unsigned int client_id, unsigned int shader_id, unsigned int heterogeneous_volume)
{
	SHADER_SET(shader_id, bool, heterogeneous_volume, heterogeneous_volume == 1)
}

unsigned int cycles_add_shader_node(unsigned int client_id, unsigned int shader_id, shadernode_type shn_type)
{
	ccl::ShaderNode* node = nullptr;
	switch (shn_type) {
	case shadernode_type::BACKGROUND:
		node = new ccl::BackgroundNode();
		break;
	case shadernode_type::DIFFUSE:
		node = new ccl::DiffuseBsdfNode();
		break;
	case shadernode_type::ANISOTROPIC:
		node = new ccl::AnisotropicBsdfNode();
		break;
	case shadernode_type::TRANSLUCENT:
		node = new ccl::TranslucentBsdfNode();
		break;
	case shadernode_type::TRANSPARENT:
		node = new ccl::TransparentBsdfNode();
		break;
	case shadernode_type::VELVET:
		node = new ccl::VelvetBsdfNode();
		break;
	case shadernode_type::TOON:
		node = new ccl::ToonBsdfNode();
		break;
	case shadernode_type::GLOSSY:
		node = new ccl::GlossyBsdfNode();
		break;
	case shadernode_type::GLASS:
		node = new ccl::GlassBsdfNode();
		break;
	case shadernode_type::REFRACTION:
		node = new ccl::RefractionBsdfNode();
		break;
	case shadernode_type::HAIR:
		node = new ccl::HairBsdfNode();
		break;
	case shadernode_type::EMISSION:
		node = new ccl::EmissionNode();
		break;
	case shadernode_type::AMBIENT_OCCLUSION:
		node = new ccl::AmbientOcclusionNode();
		break;
	case shadernode_type::ABSORPTION_VOLUME:
		node = new ccl::AbsorptionVolumeNode();
		break;
	case shadernode_type::SCATTER_VOLUME:
		node = new ccl::ScatterVolumeNode();
		break;
	case shadernode_type::SUBSURFACE_SCATTERING:
		node = new ccl::SubsurfaceScatteringNode();
		break;
	case shadernode_type::VALUE:
		node = new ccl::ValueNode();
		break;
	case shadernode_type::COLOR:
		node = new ccl::ColorNode();
		break;
	case shadernode_type::MIX_CLOSURE:
		node = new ccl::MixClosureNode();
		break;
	case shadernode_type::ADD_CLOSURE:
		node = new ccl::AddClosureNode();
		break;
	case shadernode_type::INVERT:
		node = new ccl::InvertNode();
		break;
	case shadernode_type::MIX:
		node = new ccl::MixNode();
		break;
	case shadernode_type::GAMMA:
		node = new ccl::GammaNode();
		break;
	case shadernode_type::WAVELENGTH:
		node = new ccl::WavelengthNode();
		break;
	case shadernode_type::BLACKBODY:
		node = new ccl::BlackbodyNode();
		break;
	case shadernode_type::CAMERA:
		node = new ccl::CameraNode();
		break;
	case shadernode_type::FRESNEL:
		node = new ccl::FresnelNode();
		break;
	case shadernode_type::MATH:
		node = new ccl::MathNode();
		break;
	case shadernode_type::IMAGE_TEXTURE:
		node = new ccl::ImageTextureNode();
		break;
	case shadernode_type::ENVIRONMENT_TEXTURE:
		node = new ccl::EnvironmentTextureNode();
		break;
	case shadernode_type::BRICK_TEXTURE:
		node = new ccl::BrickTextureNode();
		break;
	case shadernode_type::SKY_TEXTURE:
		node = new ccl::SkyTextureNode();
		break;
	case shadernode_type::CHECKER_TEXTURE:
		node = new ccl::CheckerTextureNode();
		break;
	case shadernode_type::NOISE_TEXTURE:
		node = new ccl::NoiseTextureNode();
		break;
	case shadernode_type::WAVE_TEXTURE:
		node = new ccl::WaveTextureNode();
		break;
	case shadernode_type::MAGIC_TEXTURE:
		node = new ccl::MagicTextureNode();
		break;
	case shadernode_type::MUSGRAVE_TEXTURE:
		node = new ccl::MusgraveTextureNode();
		break;
	case shadernode_type::TEXTURE_COORDINATE:
		node = new ccl::TextureCoordinateNode();
		break;
	case shadernode_type::BUMP:
		node = new ccl::BumpNode();
		break;
	case shadernode_type::RGBTOBW:
		node = new ccl::RGBToBWNode();
		break;
	case shadernode_type::RGBTOLUMINANCE:
		node = new ccl::RGBToLuminanceNode();
		break;
	case shadernode_type::LIGHTPATH:
		node = new ccl::LightPathNode();
		break;
	case shadernode_type::LIGHTFALLOFF:
		node = new ccl::LightFalloffNode();
		break;
	case shadernode_type::VORONOI_TEXTURE:
		node = new ccl::VoronoiTextureNode();
		break;
	case shadernode_type::LAYERWEIGHT:
		node = new ccl::LayerWeightNode();
		break;
	case shadernode_type::GEOMETRYINFO:
		node = new ccl::GeometryNode();
		break;
	case shadernode_type::COMBINE_XYZ:
		node = new ccl::CombineXYZNode();
		break;
	case shadernode_type::SEPARATE_XYZ:
		node = new ccl::SeparateXYZNode();
		break;
	case shadernode_type::HSV_SEPARATE:
		node = new ccl::SeparateHSVNode();
		break;
	case shadernode_type::HSV_COMBINE:
		node = new ccl::CombineHSVNode();
		break;
	case shadernode_type::RGB_SEPARATE:
		node = new ccl::SeparateRGBNode();
		break;
	case shadernode_type::RGB_COMBINE:
		node = new ccl::CombineRGBNode();
		break;
	case shadernode_type::MAPPING:
		node = new ccl::MappingNode();
		break;
	case shadernode_type::HOLDOUT:
		node = new ccl::HoldoutNode();
		break;
	case shadernode_type::HUE_SAT:
		node = new ccl::HSVNode();
		break;
	case shadernode_type::GRADIENT_TEXTURE:
		node = new ccl::GradientTextureNode();
		break;
	case shadernode_type::COLOR_RAMP:
		node = new ccl::RGBRampNode();
		break;
	case shadernode_type::VECT_MATH:
		node = new ccl::VectorMathNode();
		break;
	case shadernode_type::MATRIX_MATH:
		node = new ccl::MatrixMathNode();
		break;
	case shadernode_type::PRINCIPLED_BSDF:
		node = new ccl::PrincipledBsdfNode();
		break;
	case shadernode_type::ATTRIBUTE:
		node = new ccl::AttributeNode();
		break;
	case shadernode_type::NORMALMAP:
		node = new ccl::NormalMapNode();
		break;
	}

	if (node) {
		shaders[shader_id]->graph->add(node);
		return (unsigned int)(node->id);
	}
	else {
		return (unsigned int)-1;
	}
}

enum class attr_type {
	INT,
	FLOAT,
	FLOAT4,
	CHARP
};

struct attrunion {
	attr_type type;
	union {
		int i;
		float f;
		ccl::float4 f4;
		const char* cp;
	};
};

void shadernode_set_attribute(unsigned int client_id, unsigned int shader_id, unsigned int shnode_id, const char* attribute_name, attrunion v)
{
	auto attr = std::string(attribute_name);
	ccl::ShaderNode* shnode = _shader_node_find(shader_id, shnode_id);
	if (shnode) {
		for (ccl::ShaderInput* inp : shnode->inputs) {
			auto inpname = inp->name().string();
			if (ccl::string_iequals(inpname, attribute_name)) {
				switch (v.type) {
				case attr_type::INT:
					inp->set(v.i);
					logger.logit(client_id, "shader_id: ", shader_id, " -> shnode_id: ", shnode_id, " |> setting attribute: ", attribute_name, " to: ", v.i);
					break;
				case attr_type::FLOAT:
					inp->set(v.f);
					logger.logit(client_id, "shader_id: ", shader_id, " -> shnode_id: ", shnode_id, " |> setting attribute: ", attribute_name, " to: ", v.f);
					break;
				case attr_type::FLOAT4:
					ccl::float3 f3;
					f3.x = v.f4.x;
					f3.y = v.f4.y;
					f3.z = v.f4.z;
					inp->set(f3);
					logger.logit(client_id, "shader_id: ", shader_id, " -> shnode_id: ", shnode_id, " |> setting attribute: ", attribute_name, " to: ", v.f4.x, ",", v.f4.y, ",", v.f4.z);
					break;
				case attr_type::CHARP:
					inp->set(OpenImageIO::v1_3::ustring(v.cp));
					logger.logit(client_id, "shader_id: ", shader_id, " -> shnode_id: ", shnode_id, " |> setting attribute: ", attribute_name, " to: ", v.cp);
					break;
				}
				return;
			}
		}
	}
}

void _set_texture_mapping_transformation(ccl::TextureMapping& mapping, int transform_type, float x, float y, float z)
{
	switch (transform_type) {
	case 0:
		mapping.translation.x = x;
		mapping.translation.y = y;
		mapping.translation.z = z;
		break;
	case 1:
		mapping.rotation.x = x;
		mapping.rotation.y = y;
		mapping.rotation.z = z;
		break;
	case 2:
		mapping.scale.x = x;
		mapping.scale.y = y;
		mapping.scale.z = z;
		break;
	}
}

void cycles_shadernode_texmapping_set_transformation(unsigned int client_id, unsigned int shader_id, unsigned int shnode_id, shadernode_type shn_type, int transform_type, float x, float y, float z)
{
	ccl::ShaderNode* shnode = _shader_node_find(shader_id, shnode_id);
	if (shnode) {
		std::string tp{ "UNKNOWN" };
		switch (transform_type) {
		case 0:
			tp = "TRANSLATION";
			break;
		case 1:
			tp = "ROTATION";
			break;
		case 2:
			tp = "SCALE";
			break;
		}
		logger.logit(client_id, "Setting texture map transformation (", tp, ") to ", x, ",", y, ",", z, " for shadernode type ", shn_type);
		switch (shn_type) {
		case shadernode_type::MAPPING:
		{
			ccl::MappingNode* node = dynamic_cast<ccl::MappingNode*>(shnode);
			_set_texture_mapping_transformation(node->tex_mapping, transform_type, x, y, z);
		}
		break;
		case shadernode_type::ENVIRONMENT_TEXTURE:
		{
			ccl::EnvironmentTextureNode* node = dynamic_cast<ccl::EnvironmentTextureNode*>(shnode);
			_set_texture_mapping_transformation(node->tex_mapping, transform_type, x, y, z);
		}
		break;
		case shadernode_type::IMAGE_TEXTURE:
		{
			ccl::ImageTextureNode* node = dynamic_cast<ccl::ImageTextureNode*>(shnode);
			_set_texture_mapping_transformation(node->tex_mapping, transform_type, x, y, z);
		}
		break;
		case shadernode_type::GRADIENT_TEXTURE:
		{
			ccl::GradientTextureNode* node = dynamic_cast<ccl::GradientTextureNode*>(shnode);
			_set_texture_mapping_transformation(node->tex_mapping, transform_type, x, y, z);
		}
		break;
		case shadernode_type::WAVE_TEXTURE:
		{
			ccl::WaveTextureNode* node = dynamic_cast<ccl::WaveTextureNode*>(shnode);
			_set_texture_mapping_transformation(node->tex_mapping, transform_type, x, y, z);
		}
		break;
		case shadernode_type::VORONOI_TEXTURE:
		{
			ccl::VoronoiTextureNode* node = dynamic_cast<ccl::VoronoiTextureNode*>(shnode);
			_set_texture_mapping_transformation(node->tex_mapping, transform_type, x, y, z);
		}
		break;
		case shadernode_type::MUSGRAVE_TEXTURE:
		{
			ccl::MusgraveTextureNode* node = dynamic_cast<ccl::MusgraveTextureNode*>(shnode);
			_set_texture_mapping_transformation(node->tex_mapping, transform_type, x, y, z);
		}
		break;
		case shadernode_type::BRICK_TEXTURE:
		{
			ccl::BrickTextureNode* node = dynamic_cast<ccl::BrickTextureNode*>(shnode);
			_set_texture_mapping_transformation(node->tex_mapping, transform_type, x, y, z);
		}
		break;
		case shadernode_type::MAGIC_TEXTURE:
		{
			ccl::MagicTextureNode* node = dynamic_cast<ccl::MagicTextureNode*>(shnode);
			_set_texture_mapping_transformation(node->tex_mapping, transform_type, x, y, z);
		}
		break;
		case shadernode_type::NOISE_TEXTURE:
		{
			ccl::NoiseTextureNode* node = dynamic_cast<ccl::NoiseTextureNode*>(shnode);
			_set_texture_mapping_transformation(node->tex_mapping, transform_type, x, y, z);
		}
		break;
		}
	}
}

void _set_texmapping_mapping(ccl::TextureMapping& tex_mapping, ccl::TextureMapping::Mapping x, ccl::TextureMapping::Mapping y, ccl::TextureMapping::Mapping z)
{
	tex_mapping.x_mapping = x;
	tex_mapping.y_mapping = y;
	tex_mapping.z_mapping = z;
}

void cycles_shadernode_texmapping_set_mapping(unsigned int client_id, unsigned int shader_id, unsigned int shnode_id, shadernode_type shn_type, ccl::TextureMapping::Mapping x, ccl::TextureMapping::Mapping y, ccl::TextureMapping::Mapping z)
{
	ccl::ShaderNode* shnode = _shader_node_find(shader_id, shnode_id);
	if (shnode) {
		logger.logit(client_id, "Setting texture map mapping to ", x, ",", y, ",", z, " for shadernode type ", shn_type);
		switch (shn_type) {
		case shadernode_type::MAPPING:
		{
			ccl::MappingNode* node = dynamic_cast<ccl::MappingNode*>(shnode);
			_set_texmapping_mapping(node->tex_mapping, x, y, z);
		}
		break;
		case shadernode_type::ENVIRONMENT_TEXTURE:
		{
			ccl::EnvironmentTextureNode* node = dynamic_cast<ccl::EnvironmentTextureNode*>(shnode);
			_set_texmapping_mapping(node->tex_mapping, x, y, z);
		}
		break;
		case shadernode_type::IMAGE_TEXTURE:
		{
			ccl::ImageTextureNode* node = dynamic_cast<ccl::ImageTextureNode*>(shnode);
			_set_texmapping_mapping(node->tex_mapping, x, y, z);
		}
		break;
		case shadernode_type::GRADIENT_TEXTURE:
		{
			ccl::GradientTextureNode* node = dynamic_cast<ccl::GradientTextureNode*>(shnode);
			_set_texmapping_mapping(node->tex_mapping, x, y, z);
		}
		break;
		case shadernode_type::WAVE_TEXTURE:
		{
			ccl::WaveTextureNode* node = dynamic_cast<ccl::WaveTextureNode*>(shnode);
			_set_texmapping_mapping(node->tex_mapping, x, y, z);
		}
		break;
		case shadernode_type::VORONOI_TEXTURE:
		{
			ccl::VoronoiTextureNode* node = dynamic_cast<ccl::VoronoiTextureNode*>(shnode);
			_set_texmapping_mapping(node->tex_mapping, x, y, z);
		}
		break;
		case shadernode_type::MUSGRAVE_TEXTURE:
		{
			ccl::MusgraveTextureNode* node = dynamic_cast<ccl::MusgraveTextureNode*>(shnode);
			_set_texmapping_mapping(node->tex_mapping, x, y, z);
		}
		break;
		case shadernode_type::BRICK_TEXTURE:
		{
			ccl::BrickTextureNode* node = dynamic_cast<ccl::BrickTextureNode*>(shnode);
			_set_texmapping_mapping(node->tex_mapping, x, y, z);
		}
		break;
		case shadernode_type::MAGIC_TEXTURE:
		{
			ccl::MagicTextureNode* node = dynamic_cast<ccl::MagicTextureNode*>(shnode);
			_set_texmapping_mapping(node->tex_mapping, x, y, z);
		}
		break;
		case shadernode_type::NOISE_TEXTURE:
		{
			ccl::NoiseTextureNode* node = dynamic_cast<ccl::NoiseTextureNode*>(shnode);
			_set_texmapping_mapping(node->tex_mapping, x, y, z);
		}
		break;
		}
	}
}

void cycles_shadernode_texmapping_set_projection(unsigned int client_id, unsigned int shader_id, unsigned int shnode_id, shadernode_type shn_type, ccl::TextureMapping::Projection tm_projection)
{
	ccl::ShaderNode* shnode = _shader_node_find(shader_id, shnode_id);
	if (shnode) {
		logger.logit(client_id, "Setting texture map projection type to ", tm_projection, " for shadernode type ", shn_type);
		switch (shn_type) {
		case shadernode_type::MAPPING:
			ccl::MappingNode* node = dynamic_cast<ccl::MappingNode*>(shnode);
			node->tex_mapping.projection = tm_projection;
			break;
		}
	}
}

void cycles_shadernode_texmapping_set_type(unsigned int client_id, unsigned int shader_id, unsigned int shnode_id, shadernode_type shn_type, ccl::TextureMapping::Type tm_type)
{
	ccl::ShaderNode* shnode = _shader_node_find(shader_id, shnode_id);
	if (shnode) {
		logger.logit(client_id, "Setting texture map type to ", tm_type, " for shadernode type ", shn_type);
		switch (shn_type) {
		case shadernode_type::MAPPING:
			ccl::MappingNode* node = dynamic_cast<ccl::MappingNode*>(shnode);
			node->tex_mapping.type = tm_type;
			break;
		}
	}
}

/* TODO: add all enum possibilities.
 */
void cycles_shadernode_set_enum(unsigned int client_id, unsigned int shader_id, unsigned int shnode_id, shadernode_type shn_type, const char* enum_name, int value)
{
	auto ename = std::string{ enum_name };

	ccl::ShaderNode* shnode = _shader_node_find(shader_id, shnode_id);
	if (shnode) {
		switch (shn_type) {
		case shadernode_type::MATH:
		{
			ccl::MathNode* node = dynamic_cast<ccl::MathNode*>(shnode);
			node->type = (ccl::NodeMath)value;
		}
		break;
		case shadernode_type::VECT_MATH:
		{
			ccl::VectorMathNode *node = dynamic_cast<ccl::VectorMathNode*>(shnode);
			node->type = (ccl::NodeVectorMath)value;
		}
		break;
		case shadernode_type::MATRIX_MATH:
		{
			ccl::MatrixMathNode *node = dynamic_cast<ccl::MatrixMathNode*>(shnode);
			node->type = (ccl::NodeMatrixMath)value;
		}
		break;
		case shadernode_type::MIX:
		{
			ccl::MixNode* node = dynamic_cast<ccl::MixNode*>(shnode);
			node->type = (ccl::NodeMix)value;
		}
		break;
		case shadernode_type::REFRACTION:
		{
			ccl::RefractionBsdfNode* node = dynamic_cast<ccl::RefractionBsdfNode*>(shnode);
			node->distribution = (ccl::ClosureType)value;
		}
		break;
		case shadernode_type::GLOSSY:
		{
			ccl::GlossyBsdfNode* node = dynamic_cast<ccl::GlossyBsdfNode*>(shnode);
			node->distribution = (ccl::ClosureType)value;
		}
		break;
		case shadernode_type::GLASS:
		{
			ccl::GlassBsdfNode* node = dynamic_cast<ccl::GlassBsdfNode*>(shnode);
			node->distribution = (ccl::ClosureType)value;
		}
		break;
		case shadernode_type::ANISOTROPIC:
		{
			ccl::AnisotropicBsdfNode* node = dynamic_cast<ccl::AnisotropicBsdfNode*>(shnode);
			node->distribution = (ccl::ClosureType)value;
		}
		break;
		case shadernode_type::WAVE_TEXTURE:
		{
			if (ename == "wave") {
				ccl::WaveTextureNode* node = dynamic_cast<ccl::WaveTextureNode*>(shnode);
				node->type = (ccl::NodeWaveType)value;
			}
			if (ename == "profile") {
				ccl::WaveTextureNode* node = dynamic_cast<ccl::WaveTextureNode*>(shnode);
				node->profile = (ccl::NodeWaveProfile)value;
			}
		}
		break;
		case shadernode_type::VORONOI_TEXTURE:
		{
			ccl::VoronoiTextureNode* node = dynamic_cast<ccl::VoronoiTextureNode*>(shnode);
			node->coloring = (ccl::NodeVoronoiColoring)value;
		}
		break;
		case shadernode_type::SKY_TEXTURE:
		{
			ccl::SkyTextureNode* node = dynamic_cast<ccl::SkyTextureNode*>(shnode);
			node->type = (ccl::NodeSkyType)value;
		}
		break;
		case shadernode_type::ENVIRONMENT_TEXTURE:
		{
			ccl::EnvironmentTextureNode* node = dynamic_cast<ccl::EnvironmentTextureNode*>(shnode);
			if (ename == "color_space") {
				node->color_space = (ccl::NodeImageColorSpace)value;
			}
			else if (ename == "projection") {
				node->projection = (ccl::NodeEnvironmentProjection)value;
			}
			if (ename == "interpolation") {
				node->interpolation = (ccl::InterpolationType)value;
			}
		}
		break;
		case shadernode_type::IMAGE_TEXTURE:
		{
			ccl::ImageTextureNode* node = dynamic_cast<ccl::ImageTextureNode*>(shnode);
			if (ename == "color_space") {
				node->color_space = (ccl::NodeImageColorSpace)value;
			}
			else if (ename == "projection") {
				node->projection = (ccl::NodeImageProjection)value;
			}
			break;
		}
		case shadernode_type::GRADIENT_TEXTURE:
		{
			ccl::GradientTextureNode* node = dynamic_cast<ccl::GradientTextureNode*>(shnode);
			node->type = (ccl::NodeGradientType)value;
			break;
		}
		case shadernode_type::SUBSURFACE_SCATTERING:
		{
			ccl::SubsurfaceScatteringNode* node = dynamic_cast<ccl::SubsurfaceScatteringNode*>(shnode);
			node->falloff = (ccl::ClosureType)value;
			break;
		}
		case shadernode_type::PRINCIPLED_BSDF:
		{
			ccl::PrincipledBsdfNode* node = dynamic_cast<ccl::PrincipledBsdfNode*>(shnode);
			node->distribution = (ccl::ClosureType)value;
			break;
		}
		case shadernode_type::NORMALMAP:
		{
			ccl::NormalMapNode* node = dynamic_cast<ccl::NormalMapNode*>(shnode);
			node->space = (ccl::NodeNormalMapSpace)value;
			break;
		}
		}
	}
}

CCImage* find_existing_ccimage(std::string imgname, unsigned int width, unsigned int height, unsigned int depth, unsigned int channels, bool is_float)
{
	CCImage* existing_image = nullptr;
	for (CCImage* im : images) {
		if (im->filename == imgname
			&& im->width == (int)width
			&& im->height == (int)height
			&& im->depth == (int)depth
			&& im->channels == (int)channels
			&& im->is_float == is_float
			) {
			existing_image = im;
			break;
		}
	}
	return existing_image;
}

template <class T>
CCImage* get_ccimage(std::string imgname, T* img, unsigned int width, unsigned int height, unsigned int depth, unsigned int channels, bool is_float)
{
	CCImage* existing_image = find_existing_ccimage(imgname, width, height, depth, channels, is_float);
	CCImage* nimg = existing_image ? existing_image : new CCImage();
	if (!existing_image) {
		T* imgdata = new T[width*height*channels*depth];
		memcpy(imgdata, img, sizeof(T)*width*height*channels*depth);
		nimg->builtin_data = imgdata;
		nimg->filename = imgname;
		nimg->width = (int)width;
		nimg->height = (int)height;
		nimg->depth = (int)depth;
		nimg->channels = (int)channels;
		nimg->is_float = is_float;
		images.push_back(nimg);
	}
	else {
		memcpy(existing_image->builtin_data, img, sizeof(T)*width*height*channels*depth);
	}


	return nimg;
}

void cycles_shadernode_set_member_float_img(unsigned int client_id, unsigned int scene_id, unsigned int shader_id, unsigned int shnode_id, shadernode_type shn_type, const char* member_name, const char* img_name, float* img, unsigned int width, unsigned int height, unsigned int depth, unsigned int channels)
{
	SCENE_FIND(scene_id)

	auto mname = std::string{ member_name };
	auto imname = std::string{ img_name };

	ccl::ShaderNode* shnode = _shader_node_find(shader_id, shnode_id);
	if (shnode) {
		switch (shn_type) {
		case shadernode_type::IMAGE_TEXTURE:
		{
			CCImage* nimg = get_ccimage<float>(imname, img, width, height, depth, channels, true);
			ccl::ImageTextureNode* imtex = dynamic_cast<ccl::ImageTextureNode*>(shnode);
			imtex->builtin_data = nimg;
			imtex->filename = nimg->filename;
			sce->image_manager->tag_reload_image(imname);
		}
		break;
		case shadernode_type::ENVIRONMENT_TEXTURE:
		{
			CCImage* nimg = get_ccimage<float>(imname, img, width, height, depth, channels, true);
			ccl::EnvironmentTextureNode* envtex = dynamic_cast<ccl::EnvironmentTextureNode*>(shnode);
			envtex->builtin_data = nimg;
			envtex->filename = nimg->filename;
			sce->image_manager->tag_reload_image(imname);
		}
		break;
		}
	}

	SCENE_FIND_END()
}

void cycles_shadernode_set_member_byte_img(unsigned int client_id, unsigned int scene_id, unsigned int shader_id, unsigned int shnode_id, shadernode_type shn_type, const char* member_name, const char* img_name, unsigned char* img, unsigned int width, unsigned int height, unsigned int depth, unsigned int channels)
{
	SCENE_FIND(scene_id)

	auto mname = std::string{ member_name };
	auto imname = std::string{ img_name };
	ccl::ShaderNode* shnode = _shader_node_find(shader_id, shnode_id);
	if (shnode) {
		switch (shn_type) {
		case shadernode_type::IMAGE_TEXTURE:
		{
			CCImage* nimg = get_ccimage<unsigned char>(imname, img, width, height, depth, channels, false);
			ccl::ImageTextureNode* imtex = dynamic_cast<ccl::ImageTextureNode*>(shnode);
			imtex->builtin_data = nimg;
			imtex->filename = nimg->filename;
			sce->image_manager->tag_reload_image(imname);
		}
		break;
		case shadernode_type::ENVIRONMENT_TEXTURE:
		{
			CCImage* nimg = get_ccimage<unsigned char>(imname, img, width, height, depth, channels, false);
			ccl::EnvironmentTextureNode* envtex = dynamic_cast<ccl::EnvironmentTextureNode*>(shnode);
			envtex->builtin_data = nimg;
			envtex->filename = nimg->filename;
			sce->image_manager->tag_reload_image(imname);
		}
		break;
		}
	}

	SCENE_FIND_END()
}

void cycles_shadernode_set_member_bool(unsigned int client_id, unsigned int shader_id, unsigned int shnode_id, shadernode_type shn_type, const char* member_name, bool value)
{
	auto mname = std::string{ member_name };
	ccl::ShaderNode* shnode = _shader_node_find(shader_id, shnode_id);
	if (shnode) {
		switch (shn_type) {
		case shadernode_type::MATH:
		{
			ccl::MathNode* mnode = dynamic_cast<ccl::MathNode*>(shnode);
			mnode->use_clamp = value;
		}
		break;
		case shadernode_type::MAPPING:
		{
			ccl::MappingNode* mapping = dynamic_cast<ccl::MappingNode*>(shnode);
			if (mname == "useminmax") {
				mapping->tex_mapping.use_minmax = value;
			}
		}
		break;
		case shadernode_type::COLOR_RAMP:
		{
			ccl::RGBRampNode* colorramp = dynamic_cast<ccl::RGBRampNode*>(shnode);
			if (mname == "interpolate")
			{
				colorramp->interpolate = value;
			}
		}
		break;
		case shadernode_type::BUMP:
		{
			ccl::BumpNode* bump = dynamic_cast<ccl::BumpNode*>(shnode);
			if (mname == "invert") {
				bump->invert = value;
			}
		}
		break;
		case shadernode_type::IMAGE_TEXTURE:
		{
			ccl::ImageTextureNode* imgtex = dynamic_cast<ccl::ImageTextureNode*>(shnode);
			if (mname == "use_alpha") {
				imgtex->use_alpha = value;
			}
			else if (mname == "is_linear") {
				imgtex->is_linear = value;
			}
		}
		break;
		case shadernode_type::ENVIRONMENT_TEXTURE:
		{
			ccl::EnvironmentTextureNode* envtex = dynamic_cast<ccl::EnvironmentTextureNode*>(shnode);
			if (mname == "is_linear") {
				envtex->is_linear = value;
			}
		}
		break;
		case shadernode_type::TEXTURE_COORDINATE:
		{
			ccl::TextureCoordinateNode* texco = dynamic_cast<ccl::TextureCoordinateNode*>(shnode);
			if (mname == "use_transform") {
				texco->use_transform = value;
			}
		}
		break;
		case shadernode_type::MIX:
		{
			ccl::MixNode* mix = dynamic_cast<ccl::MixNode*>(shnode);
			if (mname == "use_clamp") {
				mix->use_clamp = value;
			}
		}
		break;
		}
	}
}

void cycles_shadernode_set_member_int(unsigned int client_id, unsigned int shader_id, unsigned int shnode_id, shadernode_type shn_type, const char* member_name, int value)
{
	auto mname = std::string{ member_name };
	ccl::ShaderNode* shnode = _shader_node_find(shader_id, shnode_id);
	if (shnode) {
		switch (shn_type) {
		case shadernode_type::BRICK_TEXTURE:
		{
			ccl::BrickTextureNode* bricknode = dynamic_cast<ccl::BrickTextureNode*>(shnode);
			if (mname == "offset_frequency")
				bricknode->offset_frequency = value;
			else if (mname == "squash_frequency")
				bricknode->squash_frequency = value;
		}
		break;
		case shadernode_type::IMAGE_TEXTURE:
		{
			ccl::ImageTextureNode* imgnode = dynamic_cast<ccl::ImageTextureNode*>(shnode);
			if (mname == "interpolation") {
				imgnode->interpolation = (ccl::InterpolationType)value;
			}
			if (mname == "extension") {
				imgnode->extension = (ccl::ExtensionType)value;
			}
		}
		break;
		case shadernode_type::MAGIC_TEXTURE:
		{
			ccl::MagicTextureNode* envnode = dynamic_cast<ccl::MagicTextureNode*>(shnode);
			if (mname == "depth") {
				envnode->depth = value;
			}
		}
		break;
		}
	}
}


void cycles_shadernode_set_member_float(unsigned int client_id, unsigned int shader_id, unsigned int shnode_id, shadernode_type shn_type, const char* member_name, float value)
{
	auto mname = std::string{ member_name };

	ccl::ShaderNode* shnode = _shader_node_find(shader_id, shnode_id);
	if (shnode) {
		switch (shn_type) {
		case shadernode_type::VALUE:
		{
			ccl::ValueNode* valuenode = dynamic_cast<ccl::ValueNode*>(shnode);
			valuenode->value = value;
		}
		break;
		case shadernode_type::IMAGE_TEXTURE:
		{
			ccl::ImageTextureNode* imtexnode = dynamic_cast<ccl::ImageTextureNode*>(shnode);
			if (mname == "projection_blend") {
				imtexnode->projection_blend = value;
			}
		}
		break;
		case shadernode_type::BRICK_TEXTURE:
		{
			ccl::BrickTextureNode* bricknode = dynamic_cast<ccl::BrickTextureNode*>(shnode);
			if (mname == "offset")
				bricknode->offset = value;
			else if (mname == "squash")
				bricknode->squash = value;
		}
		break;
		case shadernode_type::SKY_TEXTURE:
		{
			ccl::SkyTextureNode* skynode = dynamic_cast<ccl::SkyTextureNode*>(shnode);
			if (mname == "turbidity")
				skynode->turbidity = value;
			else if (mname == "ground_albedo")
				skynode->ground_albedo = value;
		}
		break;
		}
	}
}

void cycles_shadernode_set_member_vec4_at_index(unsigned int client_id, unsigned int shader_id, unsigned int shnode_id, shadernode_type shn_type, const char* member_name, float x, float y, float z, float w, int index)
{
	auto mname = std::string{ member_name };

	ccl::ShaderNode* shnode = _shader_node_find(shader_id, shnode_id);
	if (shnode) {
		switch (shn_type) {
		case shadernode_type::COLOR_RAMP:
		{
			ccl::RGBRampNode* colorramp = dynamic_cast<ccl::RGBRampNode*>(shnode);
			if (colorramp->ramp.capacity() < index + 1) {
				colorramp->ramp.resize(index + 1);
				colorramp->ramp_alpha.resize(index + 1);
			}
			colorramp->ramp[index] = ccl::make_float3(x, y, z);
			colorramp->ramp_alpha[index] = w;
		}
		break;
		case shadernode_type::TEXTURE_COORDINATE:
		{
			ccl::TextureCoordinateNode* texco = dynamic_cast<ccl::TextureCoordinateNode*>(shnode);
			if (index == 0) {
				texco->ob_tfm.x.x = x;
				texco->ob_tfm.x.y = y;
				texco->ob_tfm.x.z = z;
				texco->ob_tfm.x.w = w;
			}
			if (index == 1) {
				texco->ob_tfm.y.x = x;
				texco->ob_tfm.y.y = y;
				texco->ob_tfm.y.z = z;
				texco->ob_tfm.y.w = w;
			}
			if (index == 2) {
				texco->ob_tfm.z.x = x;
				texco->ob_tfm.z.y = y;
				texco->ob_tfm.z.z = z;
				texco->ob_tfm.z.w = w;
			}
			if (index == 3) {
				texco->ob_tfm.w.x = x;
				texco->ob_tfm.w.y = y;
				texco->ob_tfm.w.z = z;
				texco->ob_tfm.w.w = w;
			}
		}
		break;
		case shadernode_type::MATRIX_MATH:
		{
			ccl::MatrixMathNode* matmath = dynamic_cast<ccl::MatrixMathNode*>(shnode);
			if (index == 0) {
				matmath->tfm.x.x = x;
				matmath->tfm.x.y = y;
				matmath->tfm.x.z = z;
				matmath->tfm.x.w = w;
			}
			if (index == 1) {
				matmath->tfm.y.x = x;
				matmath->tfm.y.y = y;
				matmath->tfm.y.z = z;
				matmath->tfm.y.w = w;
			}
			if (index == 2) {
				matmath->tfm.z.x = x;
				matmath->tfm.z.y = y;
				matmath->tfm.z.z = z;
				matmath->tfm.z.w = w;
			}
			if (index == 3) {
				matmath->tfm.w.x = x;
				matmath->tfm.w.y = y;
				matmath->tfm.w.z = z;
				matmath->tfm.w.w = w;
			}
		}
		break;
		}
	}
}

void cycles_shadernode_set_member_vec(unsigned int client_id, unsigned int shader_id, unsigned int shnode_id, shadernode_type shn_type, const char* member_name, float x, float y, float z)
{
	auto mname = std::string{ member_name };

	ccl::ShaderNode* shnode = _shader_node_find(shader_id, shnode_id);
	if (shnode) {
		switch (shn_type) {
		case shadernode_type::COLOR:
		{
			ccl::ColorNode* colnode = dynamic_cast<ccl::ColorNode*>(shnode);
			colnode->value.x = x;
			colnode->value.y = y;
			colnode->value.z = z;
		}
		break;
		case shadernode_type::SKY_TEXTURE:
		{
			ccl::SkyTextureNode* sunnode = dynamic_cast<ccl::SkyTextureNode*>(shnode);
			sunnode->sun_direction.x = x;
			sunnode->sun_direction.y = y;
			sunnode->sun_direction.z = z;
		}
		break;
		case shadernode_type::MAPPING:
		{
			ccl::MappingNode* mapping = dynamic_cast<ccl::MappingNode*>(shnode);
			if (mname == "min") {
				mapping->tex_mapping.min.x = x;
				mapping->tex_mapping.min.y = y;
				mapping->tex_mapping.min.z = z;
			}
			else if (mname == "max") {
				mapping->tex_mapping.max.x = x;
				mapping->tex_mapping.max.y = y;
				mapping->tex_mapping.max.z = z;
			}
		}
		break;
		}
	}
}

void cycles_shadernode_set_member_string(unsigned int client_id, unsigned int shader_id, unsigned int shnode_id, shadernode_type shn_type, const char* member_name, const char* value)
{
	auto mname = std::string{ member_name };
	auto mval = std::string{ value };

	ccl::ShaderNode* shnode = _shader_node_find(shader_id, shnode_id);
	if (shnode)
	{
		switch (shn_type)
		{
			case shadernode_type::ATTRIBUTE:
				ccl::AttributeNode* attrn = dynamic_cast<ccl::AttributeNode*>(shnode);
				attrn->attribute = mval;
				break;
		}
	}
}

/*
Set an integer attribute with given name to value. shader_id is the global shader ID.
*/
void cycles_shadernode_set_attribute_int(unsigned int client_id, unsigned int shader_id, unsigned int shnode_id, const char* attribute_name, int value)
{
	attrunion v{ attr_type::INT };
	v.i = value;
	shadernode_set_attribute(client_id, shader_id, shnode_id, attribute_name, v);
}

/*
Set a float attribute with given name to value. shader_id is the global shader ID.
*/
void cycles_shadernode_set_attribute_float(unsigned int client_id, unsigned int shader_id, unsigned int shnode_id, const char* attribute_name, float value)
{
	attrunion v{ attr_type::FLOAT };
	v.f = value;
	shadernode_set_attribute(client_id, shader_id, shnode_id, attribute_name, v);
}

/*
Set a vector of floats attribute with given name to x, y and z. shader_id is the global shader ID.
*/
void cycles_shadernode_set_attribute_vec(unsigned int client_id, unsigned int shader_id, unsigned int shnode_id, const char* attribute_name, float x, float y, float z)
{
	attrunion v{ attr_type::FLOAT4 };
	v.f4.x = x;
	v.f4.y = y;
	v.f4.z = z;
	shadernode_set_attribute(client_id, shader_id, shnode_id, attribute_name, v);
}

/*
Set a std::string attribute with given name to value. shader_id is the global shader ID.
*/
void cycles_shadernode_set_attribute_string(unsigned int client_id, unsigned int shader_id, unsigned int shnode_id, const char* attribute_name, const char* value)
{
	attrunion v;
	v.type = attr_type::CHARP;
	v.cp = value;
	shadernode_set_attribute(client_id, shader_id, shnode_id, attribute_name, v);
}

void cycles_shader_connect_nodes(unsigned int client_id, unsigned int shader_id, unsigned int from_id, const char* from, unsigned int to_id, const char* to)
{
	CCShader* sh = shaders[shader_id];
	auto shfrom = sh->graph->nodes.begin();
	auto shfrom_end = sh->graph->nodes.end();
	auto shto = sh->graph->nodes.begin();
	auto shto_end = sh->graph->nodes.end();

	while (shfrom != shfrom_end) {
		if ((*shfrom)->id == from_id) {
			break;
		}
		++shfrom;
	}

	while (shto != shto_end) {
		if ((*shto)->id == to_id) {
			break;
		}
		++shto;
	}

	if (shfrom == shfrom_end || shto == shto_end) {
		return; // TODO: figure out what to do on errors like this
	}
	logger.logit(client_id, "Shader ", shader_id, " :: ", from_id, ":", from, " -> ", to_id, ":", to);

	sh->graph->connect((*shfrom)->output(from), (*shto)->input(to));
}

