#version 400 core


out vec4 fragColor;
uniform sampler2D tex;
in vec2 textureCords;
void main()
{
    fragColor =   texture(tex,textureCords);
}