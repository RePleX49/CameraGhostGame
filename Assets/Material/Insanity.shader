// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Insanity"
{
	Properties
	{
		_Desaturation("Desaturation", Range( 0 , 1.5)) = 0
		_Opacity("Opacity", Range( 0 , 100)) = 0
		_Scale("Scale", Float) = 4.91
		_Blotches("Blotches", Range( 0 , 1)) = 0
		_Vignette("Vignette", Range( 0 , 3)) = 2.36
		_Amplitude("Amplitude", Float) = 1
		_Offset("Offset", Range( -0.15 , 0.15)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#if defined(UNITY_STEREO_INSTANCING_ENABLED) || defined(UNITY_STEREO_MULTIVIEW_ENABLED)
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex);
		#else
		#define ASE_DECLARE_SCREENSPACE_TEXTURE(tex) UNITY_DECLARE_SCREENSPACE_TEXTURE(tex)
		#endif
		struct Input
		{
			float4 screenPos;
			float2 uv_texcoord;
		};

		ASE_DECLARE_SCREENSPACE_TEXTURE( _GrabTexture )
		uniform float _Amplitude;
		uniform float _Scale;
		uniform float _Blotches;
		uniform float _Desaturation;
		uniform float _Vignette;
		uniform float _Offset;
		uniform float _Opacity;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 screenColor1 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,ase_grabScreenPos.xy/ase_grabScreenPos.w);
			float4 appendResult68 = (float4(( frac( _Time.y ) * _Amplitude ) , 0.0 , 0.0 , 0.0));
			float2 panner66 = ( 1.0 * _Time.y * appendResult68.xy + i.uv_texcoord);
			float simplePerlin2D24 = snoise( panner66*_Scale );
			simplePerlin2D24 = simplePerlin2D24*0.5 + 0.5;
			float clampResult27 = clamp( ( simplePerlin2D24 * 2.4 ) , 0.0 , 0.53 );
			float ifLocalVar31 = 0;
			if( clampResult27 > 0.32 )
				ifLocalVar31 = 1.0;
			else if( clampResult27 < 0.32 )
				ifLocalVar31 = 0.0;
			float ifLocalVar53 = 0;
			if( ( 1.0 - ifLocalVar31 ) > 0.32 )
				ifLocalVar53 = 1.0;
			else if( ( 1.0 - ifLocalVar31 ) < 0.32 )
				ifLocalVar53 = 0.0;
			float temp_output_39_0 = ( ifLocalVar53 * _Blotches );
			float4 temp_cast_1 = (temp_output_39_0).xxxx;
			float3 desaturateInitialColor7 = ( screenColor1 - temp_cast_1 ).rgb;
			float desaturateDot7 = dot( desaturateInitialColor7, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar7 = lerp( desaturateInitialColor7, desaturateDot7.xxx, _Desaturation );
			float temp_output_57_0 = ( ( i.uv_texcoord.x + -0.5 ) * _Vignette );
			float temp_output_58_0 = ( ( i.uv_texcoord.y + -0.5 ) * _Vignette );
			float4 temp_cast_4 = (( ( temp_output_57_0 * temp_output_57_0 ) + ( temp_output_58_0 * temp_output_58_0 ) )).xxxx;
			o.Albedo = ( ( float4( desaturateVar7 , 0.0 ) + (float4( 0,0,0,0 ) + (( screenColor1 * temp_output_39_0 ) - float4( 0,0,0,0 )) * (float4( 0.3584906,0.3584906,0.3584906,0 ) - float4( 0,0,0,0 )) / (float4( 1,1,1,0 ) - float4( 0,0,0,0 ))) ) - temp_cast_4 ).rgb;
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 temp_cast_6 = (_Offset).xxxx;
			float4 screenColor81 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( ase_grabScreenPosNorm - temp_cast_6 ).xy);
			float3 desaturateInitialColor85 = screenColor81.rgb;
			float desaturateDot85 = dot( desaturateInitialColor85, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar85 = lerp( desaturateInitialColor85, desaturateDot85.xxx, _Desaturation );
			float3 ifLocalVar103 = 0;
			if( _Desaturation >= ( _Desaturation * 0.5 ) )
				ifLocalVar103 = desaturateVar85;
			float4 temp_cast_9 = (( _Offset + 0.01 )).xxxx;
			float4 screenColor100 = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_GrabTexture,( ase_grabScreenPosNorm - temp_cast_9 ).xy);
			float3 desaturateInitialColor101 = screenColor100.rgb;
			float desaturateDot101 = dot( desaturateInitialColor101, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar101 = lerp( desaturateInitialColor101, desaturateDot101.xxx, _Desaturation );
			float3 ifLocalVar106 = 0;
			if( _Desaturation >= ( _Desaturation * 0.6 ) )
				ifLocalVar106 = desaturateVar101;
			float layeredBlendVar105 = 0.5;
			float3 layeredBlend105 = ( lerp( ifLocalVar103,ifLocalVar106 , layeredBlendVar105 ) );
			o.Emission = layeredBlend105;
			o.Alpha = (0.0 + (( ( _Opacity - 100.0 ) * -1.0 ) - 0.0) * (0.8 - 0.0) / (100.0 - 0.0));
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18712
0;0;1706.667;899;1212.539;594.8563;1;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;65;-3287.825,301.1582;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-3242.698,504.3994;Inherit;False;Property;_Amplitude;Amplitude;5;0;Create;True;0;0;0;False;0;False;1;30;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FractNode;80;-3110.926,349.2104;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-2965.857,392.0938;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;22;-2651.847,102.6275;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;68;-2731.656,302.9445;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.PannerNode;66;-2484.952,358.6949;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-2483.137,509.7015;Inherit;False;Property;_Scale;Scale;2;0;Create;True;0;0;0;False;0;False;4.91;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;24;-2260.773,368.7683;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-2027.906,331.9432;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;2.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;27;-1816.005,346.0722;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.53;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-1822.462,605.6574;Inherit;False;Constant;_Float2;Float 2;5;0;Create;True;0;0;0;False;0;False;0.32;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-1677.825,739.9172;Inherit;False;Constant;_Float4;Float 4;5;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-1830.07,717.3622;Inherit;False;Constant;_Float3;Float 3;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;31;-1552.677,433.0339;Inherit;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;95;-1740.857,-474.9137;Inherit;False;Property;_Offset;Offset;6;0;Create;True;0;0;0;False;0;False;0;0;-0.15;0.15;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;51;-1200.268,834.6727;Inherit;False;Constant;_Float1;Float 1;5;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-1352.512,812.1177;Inherit;False;Constant;_Float6;Float 6;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;37;-1297.355,437.8726;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;50;-1344.904,700.4129;Inherit;False;Constant;_Float5;Float 5;5;0;Create;True;0;0;0;False;0;False;0.32;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;53;-1131.339,507.3524;Inherit;True;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GrabScreenPosition;96;-1736.267,-694.0368;Inherit;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;102;-1526.931,-324.2292;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-828.8247,558.3681;Inherit;False;Property;_Blotches;Blotches;3;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;93;-1381.74,-709.0013;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ScreenColorNode;1;-1390.294,-210.6503;Inherit;False;Global;_GrabScreen0;Grab Screen 0;0;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;98;-1362.258,-418.8489;Inherit;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;54;-2380.843,-150.1497;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-2303.297,-246.234;Inherit;False;Property;_Vignette;Vignette;4;0;Create;True;0;0;0;False;0;False;2.36;2.36;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-894.7021,428.3568;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.46;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-2374.721,72.30547;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;-0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;100;-1181.884,-440.9866;Inherit;False;Global;_GrabScreen2;Grab Screen 1;6;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScreenColorNode;81;-1201.366,-731.1389;Inherit;False;Global;_GrabScreen1;Grab Screen 1;6;0;Create;True;0;0;0;False;0;False;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-713.2864,-143.1811;Inherit;False;Property;_Desaturation;Desaturation;0;0;Create;True;0;0;0;False;0;False;0;0;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-2131.864,-150.313;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-2131.864,76.22382;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;-698.5052,400.7725;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;38;-784.8473,-40.86238;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-804.2028,182.2527;Inherit;False;Property;_Opacity;Opacity;1;0;Create;True;0;0;0;False;0;False;0;0;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;7;-577.1266,-49.73207;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;109;-642.4565,287.8783;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;100;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;-724.3145,-308.2263;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-1909.409,-148.2721;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;85;-906.1596,-703.3364;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-1899.204,78.26469;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;-758.3895,-590.4269;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;101;-886.6774,-413.1841;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;46;-468.5063,472.3237;Inherit;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0.3584906,0.3584906,0.3584906,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ConditionalIfNode;103;-538.5771,-672.7443;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ConditionalIfNode;106;-554.0833,-397.1534;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;62;-1668.585,-3.370254;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;45;-399.6057,29.08388;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;110;-486.4565,284.8783;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LayeredBlendNode;105;-358.6893,-439.6268;Inherit;False;6;0;FLOAT;0.5;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TFHCRemapNode;108;-313.6914,254.5861;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;100;False;3;FLOAT;0;False;4;FLOAT;0.8;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;64;-273.9503,-52.83434;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;100,-33;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Insanity;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;80;0;65;0
WireConnection;72;0;80;0
WireConnection;72;1;71;0
WireConnection;68;0;72;0
WireConnection;66;0;22;0
WireConnection;66;2;68;0
WireConnection;24;0;66;0
WireConnection;24;1;25;0
WireConnection;28;0;24;0
WireConnection;27;0;28;0
WireConnection;31;0;27;0
WireConnection;31;1;32;0
WireConnection;31;2;34;0
WireConnection;31;4;33;0
WireConnection;37;0;31;0
WireConnection;53;0;37;0
WireConnection;53;1;50;0
WireConnection;53;2;51;0
WireConnection;53;4;52;0
WireConnection;102;0;95;0
WireConnection;93;0;96;0
WireConnection;93;1;95;0
WireConnection;98;0;96;0
WireConnection;98;1;102;0
WireConnection;54;0;22;1
WireConnection;39;0;53;0
WireConnection;39;1;40;0
WireConnection;55;0;22;2
WireConnection;100;0;98;0
WireConnection;81;0;93;0
WireConnection;57;0;54;0
WireConnection;57;1;61;0
WireConnection;58;0;55;0
WireConnection;58;1;61;0
WireConnection;41;0;1;0
WireConnection;41;1;39;0
WireConnection;38;0;1;0
WireConnection;38;1;39;0
WireConnection;7;0;38;0
WireConnection;7;1;4;0
WireConnection;109;0;2;0
WireConnection;107;0;4;0
WireConnection;59;0;57;0
WireConnection;59;1;57;0
WireConnection;85;0;81;0
WireConnection;85;1;4;0
WireConnection;60;0;58;0
WireConnection;60;1;58;0
WireConnection;104;0;4;0
WireConnection;101;0;100;0
WireConnection;101;1;4;0
WireConnection;46;0;41;0
WireConnection;103;0;4;0
WireConnection;103;1;104;0
WireConnection;103;2;85;0
WireConnection;103;3;85;0
WireConnection;106;0;4;0
WireConnection;106;1;107;0
WireConnection;106;2;101;0
WireConnection;106;3;101;0
WireConnection;62;0;59;0
WireConnection;62;1;60;0
WireConnection;45;0;7;0
WireConnection;45;1;46;0
WireConnection;110;0;109;0
WireConnection;105;1;103;0
WireConnection;105;2;106;0
WireConnection;108;0;110;0
WireConnection;64;0;45;0
WireConnection;64;1;62;0
WireConnection;0;0;64;0
WireConnection;0;2;105;0
WireConnection;0;9;108;0
ASEEND*/
//CHKSM=07D6DEFC3DFD6176852F7C88A17EA80041528249