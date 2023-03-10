/*
 Copyright (c) 2013 yvt
 
 This file is part of OpenSpades.
 
 OpenSpades is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 OpenSpades is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with OpenSpades.  If not, see <http://www.gnu.org/licenses/>.
 
 */



varying vec4 color;
varying vec2 ambientOcclusionCoord;
varying vec2 detailCoord;
varying vec3 fogDensity;

uniform sampler2D ambientOcclusionTexture;
uniform sampler2D detailTexture;
uniform vec3 fogColor;
uniform bool glowDetect;

vec3 EvaluateSunLight();
vec3 EvaluateAmbientLight(float detailAmbientOcclusion);
//void VisibilityOfSunLight_Model_Debug();

// Threshold for what block colors should be regarded as a light source
const float GLOW_LIGHT_THRESHOLD = 255.0;

// Fix floating point comparison errors
// This decimal should never be a problem, as colors are passed to the shader as integers
const float GLOW_THRES_FLOAT_ERROR = 0.01;
const float GLOW_THRES_LINEARIZED = (GLOW_LIGHT_THRESHOLD - GLOW_THRES_FLOAT_ERROR) / 255.0;

bool glglowhelper_isLight(vec3 rgb) {
	return (rgb.x >= GLOW_THRES_LINEARIZED ||
			rgb.y >= GLOW_THRES_LINEARIZED ||
			rgb.z >= GLOW_THRES_LINEARIZED);
}

void main() {
	// color is linear
	gl_FragColor = vec4(color.xyz, 1.);
	
	if (!glglowhelper_isLight(color.xyz)) {
		vec3 shading = vec3(color.w);
		shading *= EvaluateSunLight();
	
		float ao = texture2D(ambientOcclusionTexture, ambientOcclusionCoord).x;
		
		shading += EvaluateAmbientLight(ao);
		
		// apply diffuse shading
		gl_FragColor.xyz *= shading;
	}
	
	// apply fog
	gl_FragColor.xyz = mix(gl_FragColor.xyz, fogColor, fogDensity);
	
#if !LINEAR_FRAMEBUFFER
	// gamma correct
	gl_FragColor.xyz = sqrt(gl_FragColor.xyz);
#endif
}

