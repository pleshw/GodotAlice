shader_type canvas_item;

uniform bool active = false;

void fragment() {
	if(active){
	    COLOR = texture(TEXTURE, UV);
    	float avg = (COLOR.r + COLOR.g + COLOR.b) / 3.0;
    	COLOR.rgb = vec3(avg);
	}
}