static const GLchar* fs_src =
"#version 330                                                     \n"

"uniform sampler2D tex;                                           \n"
"uniform vec4 subsize;                                            \n"
"uniform float alpha;                                             \n"

"out vec4 Color;                                                  \n"

"void main()                                                      \n"
"{                                                                \n"
"  if(gl_FragCoord.y<subsize.z || gl_FragCoord.y>subsize.w)       \n"
"    discard;                                                     \n"
"  vec2 vp = vec2(subsize.y, subsize.w - subsize.z);              \n"
"  vec2 cd = vec2(gl_FragCoord.x, gl_FragCoord.y - subsize.z);    \n"
"  vec2 tc = cd / vp;                                             \n"

"  vec4 px = texture(tex, tc);                                    \n"
"  Color = vec4(px.rgb, alpha);                                   \n"
"}                                                                \n";