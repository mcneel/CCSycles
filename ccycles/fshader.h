"#version 330                                                     \n"

"uniform sampler2D tex;                                           \n"
"uniform vec2 viewport_size;                                      \n"
"uniform vec4 subsize;                                            \n"

"out vec4 Color;                                                  \n"

"void main()                                                      \n"
"{                                                                \n"
"  if(gl_FragCoord.y<subsize.z || gl_FragCoord.y>subsize.w)       \n"
"    discard;                                                     \n"
//"  vec2 tc = gl_FragCoord.xy / viewport_size;                     \n"
"  vec2 vp = vec2(subsize.y, subsize.w - subsize.z);"
"  vec2 cd = vec2(gl_FragCoord.x, gl_FragCoord.y - subsize.z);"
"  vec2 tc = cd / vp;                        \n"
//"  vec2 adj = vec2(0.0, subsize.w-subsize.y);                     \n"
//"  vec2 tc = (gl_FragCoord.xy - subsize.xy) / viewport_size;      \n"

"  vec4 px = texture(tex, tc);                                    \n"
"  Color = vec4(px.rgb, 1.0);                                     \n"
//"  if(subsize.x > 2)"
//"    Color = vec4(1.0, 0.0, 0.0, 1.0);                            \n"
//"  else if(subsize.x > 1)"
//"    Color = vec4(0.0, 1.0, 0.0, 1.0);                            \n"
//"  else if(subsize.x > 0)"
//"    Color = vec4(0.0, 0.0, 1.0, 1.0);                            \n"
"}                                                                \n";
