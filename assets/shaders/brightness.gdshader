shader_type canvas_item;


uniform float factor: hint_range(0, 10) = 0.0;

void fragment(){
	float colorTotal = COLOR.r + COLOR.g + COLOR.b;
	float rPresence = (COLOR.r / colorTotal);
	float gPresence = (COLOR.g / colorTotal);
	float bPresence = (COLOR.b / colorTotal);
	COLOR.r = COLOR.r * rPresence * factor /3.0;
	COLOR.g = COLOR.g * gPresence * factor /3.0;
	COLOR.b = COLOR.b * bPresence * factor/3.0;
}