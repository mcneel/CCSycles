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

extern std::vector<CCScene> scenes;

unsigned int cycles_scene_add_mesh(unsigned int client_id, unsigned int scene_id, unsigned int shader_id)
{
	SCENE_FIND(scene_id)
		ccl::Mesh* mesh = new ccl::Mesh();

		ccl::Shader* sh = find_shader_in_scene(sce, shader_id);

		mesh->used_shaders.push_back(sh);
		sce->meshes.push_back(mesh);

		logger.logit(client_id, "Add mesh ", sce->meshes.size() - 1, " in scene ", scene_id, " using default surface shader ", shader_id);

		return (unsigned int)(sce->meshes.size() - 1);
	SCENE_FIND_END()

	return UINT_MAX;
}

unsigned int cycles_scene_add_mesh_object(unsigned int client_id, unsigned int scene_id, unsigned int object_id, unsigned int shader_id)
{
	SCENE_FIND(scene_id)
		ccl::Mesh* mesh = new ccl::Mesh();
		ccl::Shader* sh = find_shader_in_scene(sce, shader_id);
		
		ccl::Object* ob = sce->objects[object_id];
		ob->mesh = mesh;


		mesh->used_shaders.push_back(sh);
		sce->meshes.push_back(mesh);

		logger.logit(client_id, "Add mesh ", sce->meshes.size() - 1, " to object ", object_id, " in scene ", scene_id, " using default surface shader ", shader_id);

		return (unsigned int)(sce->meshes.size() - 1);
	SCENE_FIND_END()

	return UINT_MAX;
}

void cycles_mesh_set_shader(unsigned int client_id, unsigned int scene_id, unsigned int mesh_id, unsigned int shader_id)
{
	SCENE_FIND(scene_id)
		ccl::Mesh* me = sce->meshes[mesh_id];
		ccl::Shader* sh = find_shader_in_scene(sce, shader_id);

		me->used_shaders.push_back(sh);
		int idx = (int)me->used_shaders.size() - 1;

		me->shader.resize(me->triangles.size());
		for (int i = 0; i < me->triangles.size(); i++) {
			me->shader[i] = idx;
		}

		sh->tag_update(sce);
		sh->tag_used(sce);

	SCENE_FIND_END()
}

void cycles_mesh_clear(unsigned int client_id, unsigned int scene_id, unsigned int mesh_id)
{
	SCENE_FIND(scene_id)
		ccl::Mesh* me = sce->meshes[mesh_id];
		me->clear();
	SCENE_FIND_END()
}

void cycles_mesh_tag_rebuild(unsigned int client_id, unsigned int scene_id, unsigned int mesh_id)
{
	SCENE_FIND(scene_id)
		ccl::Mesh* me = sce->meshes[mesh_id];
		me->tag_update(sce, true);
	SCENE_FIND_END()
}

void cycles_mesh_set_smooth(unsigned int client_id, unsigned int scene_id, unsigned int mesh_id, unsigned int smooth)
{
	SCENE_FIND(scene_id)
		ccl::Mesh* me = sce->meshes[mesh_id];
		bool use_smooth = smooth == 1;
		me->smooth.resize(me->triangles.size());
		for (int i = 0; i < me->triangles.size(); i++) {
			me->smooth[i] = use_smooth;
		}
	SCENE_FIND_END()
}


void cycles_mesh_reserve(unsigned int client_id, unsigned int scene_id, unsigned int mesh_id, unsigned vcount, unsigned fcount)
{
	SCENE_FIND(scene_id)
		ccl::Mesh* me = sce->meshes[mesh_id];

		me->reserve_mesh(vcount, fcount);
	SCENE_FIND_END()
}

void cycles_mesh_resize(unsigned int client_id, unsigned int scene_id, unsigned int mesh_id, unsigned vcount, unsigned fcount)
{
	SCENE_FIND(scene_id)
		ccl::Mesh* me = sce->meshes[mesh_id];

		me->resize_mesh(vcount, fcount);
	SCENE_FIND_END()
}


void cycles_mesh_set_verts(unsigned int client_id, unsigned int scene_id, unsigned int mesh_id, float *verts, unsigned int vcount)
{
	SCENE_FIND(scene_id)
		ccl::Mesh* me = sce->meshes[mesh_id];

		ccl::float3 f3;

		for (int i = 0, j = 0; i < (int)vcount*3; i+=3, j++) {
			f3.x = verts[i];
			f3.y = verts[i+1];
			f3.z = verts[i+2];
			//logger.logit(client_id, "v: ", f3.x, ",", f3.y, ",", f3.z);
			me->verts[j] = f3;
		}
		me->geometry_flags = ccl::Mesh::GeometryFlags::GEOMETRY_TRIANGLES;
	SCENE_FIND_END()
}

void cycles_mesh_set_tris(unsigned int client_id, unsigned int scene_id, unsigned int mesh_id, int *faces, unsigned int fcount, unsigned int shader_id, unsigned int smooth)
{
	SCENE_FIND(scene_id)
		ccl::Mesh* me = sce->meshes[mesh_id];

		me->reserve_mesh(fcount * 3, fcount);

		for (int i = 0, j = 0; i < (int)fcount*3; i += 3, j++) {
			//logger.logit(client_id, "f: ", faces[i], ",", faces[i + 1], ",", faces[i + 2]);
			me->triangles[i] = faces[i];
			me->triangles[i + 1] = faces[i + 1];
			me->triangles[i + 2] = faces[i + 2];
			me->shader[j] = shader_id;
			me->smooth[j] = smooth == 1;
		}
		me->geometry_flags = ccl::Mesh::GeometryFlags::GEOMETRY_TRIANGLES;

		cycles_mesh_set_shader(client_id, scene_id, mesh_id, shader_id);
	SCENE_FIND_END()
}

void cycles_mesh_set_triangle(unsigned int client_id, unsigned int scene_id, unsigned int mesh_id, unsigned tri_idx, unsigned int v0, unsigned int v1, unsigned int v2, unsigned int shader_id, unsigned int smooth)
{
	SCENE_FIND(scene_id)
		ccl::Mesh* me = sce->meshes[mesh_id];
		me->triangles[tri_idx] = (int)v0;
		me->triangles[tri_idx + 1] = (int)v1;
		me->triangles[tri_idx + 2] = (int)v2;
		me->shader[tri_idx/3] = shader_id;
		me->smooth[tri_idx/3] = smooth == 1;
	SCENE_FIND_END()
}

void cycles_mesh_add_triangle(unsigned int client_id, unsigned int scene_id, unsigned int mesh_id, unsigned int v0, unsigned int v1, unsigned int v2, unsigned int shader_id, unsigned int smooth)
{
	SCENE_FIND(scene_id)
		ccl::Mesh* me = sce->meshes[mesh_id];
		me->add_triangle((int)v0, (int)v1, (int)v2, shader_id, smooth == 1);
	SCENE_FIND_END()
}

void cycles_mesh_set_uvs(unsigned int client_id, unsigned int scene_id, unsigned int mesh_id, float *uvs, unsigned int uvcount)
{
	SCENE_FIND(scene_id)
		ccl::Mesh* me = sce->meshes[mesh_id];

		ccl::Attribute* attr = me->attributes.add(ccl::ATTR_STD_UV, ccl::ustring("uvmap"));
		ccl::float3* fdata = attr->data_float3();

		ccl::float3 f3;

		for (int i = 0, j = 0; i < (int)uvcount * 2; i += 2, j++) {
			f3.x = uvs[i];
			f3.y = uvs[i + 1];
			f3.z = 0.0f;
			fdata[j] = f3;
		}
		me->geometry_flags = ccl::Mesh::GeometryFlags::GEOMETRY_TRIANGLES;
	SCENE_FIND_END()
}

void cycles_mesh_set_vertex_normals(unsigned int client_id, unsigned int scene_id, unsigned int mesh_id, float *vnormals, unsigned int vnormalcount)
{
	SCENE_FIND(scene_id)
		ccl::Mesh* me = sce->meshes[mesh_id];

		ccl::Attribute* attr = me->attributes.add(ccl::ATTR_STD_VERTEX_NORMAL);
		ccl::float3* fdata = attr->data_float3();

		ccl::float3 f3;

		for (int i = 0, j = 0; i < (int)vnormalcount * 3; i += 3, j++) {
			f3.x = vnormals[i];
			f3.y = vnormals[i + 1];
			f3.z = vnormals[i + 2];
			fdata[j] = f3;
		}
		me->geometry_flags = ccl::Mesh::GeometryFlags::GEOMETRY_TRIANGLES;
	SCENE_FIND_END()
}