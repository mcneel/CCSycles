		"#version 330                                 \n"

		"uniform sampler2D tex;                       \n"
		"uniform vec2 viewport_size;                  \n"

		"out vec4 Color;                              \n"

		"void main()                                  \n"
		"{                                            \n"
		"  vec2 tc = gl_FragCoord.xy / viewport_size; \n"

		"  vec4 px = texture(tex, tc);                \n"
		"  Color = vec4(px.rgb, 1.0);                 \n"
		//"  Color = vec4(0.2, 0.5, 0.8, 1.0);          \n"
		"}                                            \n";
