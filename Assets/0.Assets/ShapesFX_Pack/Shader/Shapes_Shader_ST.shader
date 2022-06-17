// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shapes_Shader_Pack"
{
	Properties
	{
		[NoScaleOffset]_DisplacementMask("Mask Map", 2D) = "white" {}
		_Animation_speed("Animation Speed", Range( 0 , 4)) = 1
		_NormalPush("Normal Push", Range( -1 , 4)) = 0
		_Shrink_Faces_Amplitude("Shrink Factor", Range( -2 , 3)) = 0
		[NoScaleOffset]_FrontFace_Diffuse_map("FrontFace_Diffuse_map", 2D) = "white" {}
		_OutlineTex("Outline Map", 2D) = "white" {}
		_Outline_Opacity("Outline Opacity", Range( 0 , 200)) = 1
		[HDR]_Outline_Color("Outline Color", Color) = (1,1,1,0)
		_FrontFace_Intensity("Intensity Mult", Range( 0 , 4)) = 1
		[HDR]_FrontFace_Color("FrontFace Color", Color) = (1,1,1,0)
		_PannerY("PannerY", Range( -1 , 1)) = 0
		_PannerX("PannerX", Range( -1 , 1)) = 1
		_TileX("TileX", Range( 0.05 , 10)) = 1
		_TileY("TileY", Range( 0.05 , 10)) = 1
		[Toggle]_Stretching("Stretching", Float) = 0
		_DefaultShrink("DefaultShrink", Range( 0 , 0.5)) = 0
		_DefaultOutlineOpacity("DefaultOutlineOpacity", Range( 0 , 25)) = 0
		[Toggle]_Debug_Mask("Debug_Mask", Float) = 0
		[Toggle]_ExtrudeUpFaces("ExtrudeUpFaces", Float) = 0
		[HideInInspector]_target("target", Vector) = (0,0,0,0)
		[HideInInspector]_target2("target2", Vector) = (0,0,0,0)
		_InfluenceRadius("InfluenceRadius", Float) = 0.5
		[Toggle(_TARGETMODE_ON)] _TargetMode("TargetMode", Float) = 0
		[Toggle(_DIRECTIONCHANGE_ON)] _DirectionChange("Direction Change", Float) = 0
		[HideInInspector]_target3("target3", Vector) = (0,0,0,0)
		[HideInInspector]_target4("target4", Vector) = (0,0,0,0)
		[HideInInspector]_Activate_Target_2("Activate_Target_2", Float) = 0
		[HideInInspector]_Activate_Target_4("Activate_Target_4", Float) = 1
		[HideInInspector]_Activate_Target_3("Activate_Target_3", Float) = 0
		[HideInInspector]_Activate_Target("Activate_Target", Float) = 0
		[HideInInspector] _texcoord3( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		
		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry" }
		
		Cull Back
		HLSLINCLUDE
		#pragma target 3.0
		ENDHLSL

		
		Pass
		{
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend One Zero , One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 70106

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag


			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#pragma shader_feature _DIRECTIONCHANGE_ON
			#pragma shader_feature _TARGETMODE_ON


			sampler2D _DisplacementMask;
			sampler2D _FrontFace_Diffuse_map;
			sampler2D _OutlineTex;
			CBUFFER_START( UnityPerMaterial )
			float _ExtrudeUpFaces;
			float _Animation_speed;
			float _PannerX;
			float _PannerY;
			float _Stretching;
			float _TileX;
			float _TileY;
			float3 _target;
			float _InfluenceRadius;
			float _Activate_Target;
			float3 _target2;
			float _Activate_Target_2;
			float _Activate_Target_3;
			float3 _target3;
			float _Activate_Target_4;
			float3 _target4;
			float _NormalPush;
			float _DefaultShrink;
			float _Shrink_Faces_Amplitude;
			float _Debug_Mask;
			float _FrontFace_Intensity;
			float4 _FrontFace_Color;
			float4 _OutlineTex_ST;
			float4 _Outline_Color;
			float _DefaultOutlineOpacity;
			float _Outline_Opacity;
			CBUFFER_END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#ifdef ASE_FOG
				float fogFactor : TEXCOORD0;
				#endif
				float3 ase_normal : NORMAL;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			float2 MyCustomExpression146( float3 normal )
			{
				float2 uv_matcap = normal *0.5 + float2(0.5,0.5); float2(0.5,0.5);
				return uv_matcap;
			}
			

			VertexOutput vert ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float mulTime7 = _Time.y * _Animation_speed;
				float2 appendResult77 = (float2(_PannerX , _PannerY));
				float2 uv163 = v.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 uv03 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult81 = (float2(_TileX , _TileY));
				float2 temp_output_78_0 = ( (( _Stretching )?( uv03 ):( uv163 )) * appendResult81 );
				#ifdef _DIRECTIONCHANGE_ON
				float2 staticSwitch163 = mul( float3( temp_output_78_0 ,  0.0 ), float3x3(0,1,0,1,0,0,0,0,0) ).xy;
				#else
				float2 staticSwitch163 = temp_output_78_0;
				#endif
				float2 panner4 = ( mulTime7 * appendResult77 + staticSwitch163);
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float3 temp_output_5_0_g11 = ( ( ase_worldPos - _target ) / _InfluenceRadius );
				float dotResult8_g11 = dot( temp_output_5_0_g11 , temp_output_5_0_g11 );
				float clampResult10_g11 = clamp( dotResult8_g11 , 0.0 , 1.0 );
				float3 temp_output_5_0_g9 = ( ( ase_worldPos - _target2 ) / _InfluenceRadius );
				float dotResult8_g9 = dot( temp_output_5_0_g9 , temp_output_5_0_g9 );
				float clampResult10_g9 = clamp( dotResult8_g9 , 0.0 , 1.0 );
				float3 temp_output_5_0_g10 = ( ( ase_worldPos - _target3 ) / _InfluenceRadius );
				float dotResult8_g10 = dot( temp_output_5_0_g10 , temp_output_5_0_g10 );
				float clampResult10_g10 = clamp( dotResult8_g10 , 0.0 , 1.0 );
				float3 temp_output_5_0_g12 = ( ( ase_worldPos - _target4 ) / _InfluenceRadius );
				float dotResult8_g12 = dot( temp_output_5_0_g12 , temp_output_5_0_g12 );
				float clampResult10_g12 = clamp( dotResult8_g12 , 0.0 , 1.0 );
				#ifdef _TARGETMODE_ON
				float staticSwitch161 = ( ( ( 1.0 - pow( clampResult10_g11 , 0.5 ) ) * _Activate_Target ) + ( ( 1.0 - pow( clampResult10_g9 , 0.5 ) ) * _Activate_Target_2 ) + ( _Activate_Target_3 * ( 1.0 - pow( clampResult10_g10 , 0.5 ) ) ) + ( _Activate_Target_4 * ( 1.0 - pow( clampResult10_g12 , 0.5 ) ) ) );
				#else
				float staticSwitch161 = 1.0;
				#endif
				float mask124 = ( tex2Dlod( _DisplacementMask, float4( panner4, 0, 0.0) ).r * staticSwitch161 );
				float temp_output_67_0 = step( 0.0 , 1.0 );
				float2 uv314 = v.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 uv415 = v.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				float4 appendResult17 = (float4(( uv314.x * -1.0 ) , uv314.y , uv415.x , 0.0));
				float4 temp_output_22_0 = ( float4( ( mask124 * v.ase_normal * ( _NormalPush * 0.01 ) * temp_output_67_0 ) , 0.0 ) + ( ( _DefaultShrink * appendResult17 * 0.01 ) + ( appendResult17 * _Shrink_Faces_Amplitude * mask124 * temp_output_67_0 * 0.01 ) ) );
				float2 uv4139 = v.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				
				o.ase_texcoord3.xyz = ase_worldPos;
				
				o.ase_normal = v.ase_normal;
				o.ase_texcoord1.xy = v.ase_texcoord2.xy;
				o.ase_texcoord1.zw = v.ase_texcoord1.xy;
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				o.ase_texcoord3.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = (( _ExtrudeUpFaces )?( ( temp_output_22_0 * uv4139.y ) ):( temp_output_22_0 )).xyz;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
				o.clipPos = vertexInput.positionCS;
				#ifdef ASE_FOG
				o.fogFactor = ComputeFogFactor( vertexInput.positionCS.z );
				#endif
				return o;
			}

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float3 objToViewDir159 = mul( UNITY_MATRIX_IT_MV, float4( IN.ase_normal, 0 ) ).xyz;
				float3 normalizeResult160 = normalize( objToViewDir159 );
				float3 normal146 = normalizeResult160;
				float2 localMyCustomExpression146 = MyCustomExpression146( normal146 );
				float4 tex2DNode27 = tex2D( _FrontFace_Diffuse_map, localMyCustomExpression146 );
				float4 break57 = ( tex2DNode27 * _FrontFace_Intensity * _FrontFace_Color );
				float4 appendResult56 = (float4(break57.r , break57.g , break57.b , _FrontFace_Color.a));
				float2 uv3_OutlineTex = IN.ase_texcoord1.xy * _OutlineTex_ST.xy + _OutlineTex_ST.zw;
				float temp_output_67_0 = step( 0.0 , 1.0 );
				float4 temp_output_37_0 = ( tex2D( _OutlineTex, uv3_OutlineTex ) * float4( 1,1,1,0 ) * _Outline_Color * temp_output_67_0 * float4( float3(0.3,0.3,0.3) , 0.0 ) );
				float mulTime7 = _Time.y * _Animation_speed;
				float2 appendResult77 = (float2(_PannerX , _PannerY));
				float2 uv163 = IN.ase_texcoord1.zw * float2( 1,1 ) + float2( 0,0 );
				float2 uv03 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult81 = (float2(_TileX , _TileY));
				float2 temp_output_78_0 = ( (( _Stretching )?( uv03 ):( uv163 )) * appendResult81 );
				#ifdef _DIRECTIONCHANGE_ON
				float2 staticSwitch163 = mul( float3( temp_output_78_0 ,  0.0 ), float3x3(0,1,0,1,0,0,0,0,0) ).xy;
				#else
				float2 staticSwitch163 = temp_output_78_0;
				#endif
				float2 panner4 = ( mulTime7 * appendResult77 + staticSwitch163);
				float3 ase_worldPos = IN.ase_texcoord3.xyz;
				float3 temp_output_5_0_g11 = ( ( ase_worldPos - _target ) / _InfluenceRadius );
				float dotResult8_g11 = dot( temp_output_5_0_g11 , temp_output_5_0_g11 );
				float clampResult10_g11 = clamp( dotResult8_g11 , 0.0 , 1.0 );
				float3 temp_output_5_0_g9 = ( ( ase_worldPos - _target2 ) / _InfluenceRadius );
				float dotResult8_g9 = dot( temp_output_5_0_g9 , temp_output_5_0_g9 );
				float clampResult10_g9 = clamp( dotResult8_g9 , 0.0 , 1.0 );
				float3 temp_output_5_0_g10 = ( ( ase_worldPos - _target3 ) / _InfluenceRadius );
				float dotResult8_g10 = dot( temp_output_5_0_g10 , temp_output_5_0_g10 );
				float clampResult10_g10 = clamp( dotResult8_g10 , 0.0 , 1.0 );
				float3 temp_output_5_0_g12 = ( ( ase_worldPos - _target4 ) / _InfluenceRadius );
				float dotResult8_g12 = dot( temp_output_5_0_g12 , temp_output_5_0_g12 );
				float clampResult10_g12 = clamp( dotResult8_g12 , 0.0 , 1.0 );
				#ifdef _TARGETMODE_ON
				float staticSwitch161 = ( ( ( 1.0 - pow( clampResult10_g11 , 0.5 ) ) * _Activate_Target ) + ( ( 1.0 - pow( clampResult10_g9 , 0.5 ) ) * _Activate_Target_2 ) + ( _Activate_Target_3 * ( 1.0 - pow( clampResult10_g10 , 0.5 ) ) ) + ( _Activate_Target_4 * ( 1.0 - pow( clampResult10_g12 , 0.5 ) ) ) );
				#else
				float staticSwitch161 = 1.0;
				#endif
				float mask124 = ( tex2D( _DisplacementMask, panner4 ).r * staticSwitch161 );
				float dotResult42 = dot( tex2DNode27 , float4(0.5,0.5,0.5,0.5) );
				float4 temp_output_58_0 = ( appendResult56 + ( ( ( temp_output_37_0 * _DefaultOutlineOpacity ) + ( temp_output_37_0 * mask124 * _Outline_Opacity ) ) * dotResult42 ) );
				float4 temp_cast_6 = (mask124).xxxx;
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = (( _Debug_Mask )?( temp_cast_6 ):( temp_output_58_0 )).xyz;
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

				#if _AlphaClip
					clip( Alpha - AlphaClipThreshold );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				return half4( Color, Alpha );
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 70106

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex ShadowPassVertex
			#pragma fragment ShadowPassFragment


			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#pragma shader_feature _DIRECTIONCHANGE_ON
			#pragma shader_feature _TARGETMODE_ON


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			sampler2D _DisplacementMask;
			CBUFFER_START( UnityPerMaterial )
			float _ExtrudeUpFaces;
			float _Animation_speed;
			float _PannerX;
			float _PannerY;
			float _Stretching;
			float _TileX;
			float _TileY;
			float3 _target;
			float _InfluenceRadius;
			float _Activate_Target;
			float3 _target2;
			float _Activate_Target_2;
			float _Activate_Target_3;
			float3 _target3;
			float _Activate_Target_4;
			float3 _target4;
			float _NormalPush;
			float _DefaultShrink;
			float _Shrink_Faces_Amplitude;
			float _Debug_Mask;
			float _FrontFace_Intensity;
			float4 _FrontFace_Color;
			float4 _OutlineTex_ST;
			float4 _Outline_Color;
			float _DefaultOutlineOpacity;
			float _Outline_Opacity;
			CBUFFER_END


			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			
			float3 _LightDirection;

			VertexOutput ShadowPassVertex( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float mulTime7 = _Time.y * _Animation_speed;
				float2 appendResult77 = (float2(_PannerX , _PannerY));
				float2 uv163 = v.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 uv03 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult81 = (float2(_TileX , _TileY));
				float2 temp_output_78_0 = ( (( _Stretching )?( uv03 ):( uv163 )) * appendResult81 );
				#ifdef _DIRECTIONCHANGE_ON
				float2 staticSwitch163 = mul( float3( temp_output_78_0 ,  0.0 ), float3x3(0,1,0,1,0,0,0,0,0) ).xy;
				#else
				float2 staticSwitch163 = temp_output_78_0;
				#endif
				float2 panner4 = ( mulTime7 * appendResult77 + staticSwitch163);
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float3 temp_output_5_0_g11 = ( ( ase_worldPos - _target ) / _InfluenceRadius );
				float dotResult8_g11 = dot( temp_output_5_0_g11 , temp_output_5_0_g11 );
				float clampResult10_g11 = clamp( dotResult8_g11 , 0.0 , 1.0 );
				float3 temp_output_5_0_g9 = ( ( ase_worldPos - _target2 ) / _InfluenceRadius );
				float dotResult8_g9 = dot( temp_output_5_0_g9 , temp_output_5_0_g9 );
				float clampResult10_g9 = clamp( dotResult8_g9 , 0.0 , 1.0 );
				float3 temp_output_5_0_g10 = ( ( ase_worldPos - _target3 ) / _InfluenceRadius );
				float dotResult8_g10 = dot( temp_output_5_0_g10 , temp_output_5_0_g10 );
				float clampResult10_g10 = clamp( dotResult8_g10 , 0.0 , 1.0 );
				float3 temp_output_5_0_g12 = ( ( ase_worldPos - _target4 ) / _InfluenceRadius );
				float dotResult8_g12 = dot( temp_output_5_0_g12 , temp_output_5_0_g12 );
				float clampResult10_g12 = clamp( dotResult8_g12 , 0.0 , 1.0 );
				#ifdef _TARGETMODE_ON
				float staticSwitch161 = ( ( ( 1.0 - pow( clampResult10_g11 , 0.5 ) ) * _Activate_Target ) + ( ( 1.0 - pow( clampResult10_g9 , 0.5 ) ) * _Activate_Target_2 ) + ( _Activate_Target_3 * ( 1.0 - pow( clampResult10_g10 , 0.5 ) ) ) + ( _Activate_Target_4 * ( 1.0 - pow( clampResult10_g12 , 0.5 ) ) ) );
				#else
				float staticSwitch161 = 1.0;
				#endif
				float mask124 = ( tex2Dlod( _DisplacementMask, float4( panner4, 0, 0.0) ).r * staticSwitch161 );
				float temp_output_67_0 = step( 0.0 , 1.0 );
				float2 uv314 = v.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 uv415 = v.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				float4 appendResult17 = (float4(( uv314.x * -1.0 ) , uv314.y , uv415.x , 0.0));
				float4 temp_output_22_0 = ( float4( ( mask124 * v.ase_normal * ( _NormalPush * 0.01 ) * temp_output_67_0 ) , 0.0 ) + ( ( _DefaultShrink * appendResult17 * 0.01 ) + ( appendResult17 * _Shrink_Faces_Amplitude * mask124 * temp_output_67_0 * 0.01 ) ) );
				float2 uv4139 = v.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = (( _ExtrudeUpFaces )?( ( temp_output_22_0 * uv4139.y ) ):( temp_output_22_0 )).xyz;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld(v.vertex.xyz);
				float3 normalWS = TransformObjectToWorldDir(v.ase_normal);

				float4 clipPos = TransformWorldToHClip( ApplyShadowBias( positionWS, normalWS, _LightDirection ) );

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#endif
				o.clipPos = clipPos;

				return o;
			}

			half4 ShadowPassFragment(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

				#if _AlphaClip
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0

			HLSLPROGRAM
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 70106

			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x

			#pragma vertex vert
			#pragma fragment frag


			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#pragma shader_feature _DIRECTIONCHANGE_ON
			#pragma shader_feature _TARGETMODE_ON


			sampler2D _DisplacementMask;
			CBUFFER_START( UnityPerMaterial )
			float _ExtrudeUpFaces;
			float _Animation_speed;
			float _PannerX;
			float _PannerY;
			float _Stretching;
			float _TileX;
			float _TileY;
			float3 _target;
			float _InfluenceRadius;
			float _Activate_Target;
			float3 _target2;
			float _Activate_Target_2;
			float _Activate_Target_3;
			float3 _target3;
			float _Activate_Target_4;
			float3 _target4;
			float _NormalPush;
			float _DefaultShrink;
			float _Shrink_Faces_Amplitude;
			float _Debug_Mask;
			float _FrontFace_Intensity;
			float4 _FrontFace_Color;
			float4 _OutlineTex_ST;
			float4 _Outline_Color;
			float _DefaultOutlineOpacity;
			float _Outline_Opacity;
			CBUFFER_END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			
			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float mulTime7 = _Time.y * _Animation_speed;
				float2 appendResult77 = (float2(_PannerX , _PannerY));
				float2 uv163 = v.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 uv03 = v.ase_texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult81 = (float2(_TileX , _TileY));
				float2 temp_output_78_0 = ( (( _Stretching )?( uv03 ):( uv163 )) * appendResult81 );
				#ifdef _DIRECTIONCHANGE_ON
				float2 staticSwitch163 = mul( float3( temp_output_78_0 ,  0.0 ), float3x3(0,1,0,1,0,0,0,0,0) ).xy;
				#else
				float2 staticSwitch163 = temp_output_78_0;
				#endif
				float2 panner4 = ( mulTime7 * appendResult77 + staticSwitch163);
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float3 temp_output_5_0_g11 = ( ( ase_worldPos - _target ) / _InfluenceRadius );
				float dotResult8_g11 = dot( temp_output_5_0_g11 , temp_output_5_0_g11 );
				float clampResult10_g11 = clamp( dotResult8_g11 , 0.0 , 1.0 );
				float3 temp_output_5_0_g9 = ( ( ase_worldPos - _target2 ) / _InfluenceRadius );
				float dotResult8_g9 = dot( temp_output_5_0_g9 , temp_output_5_0_g9 );
				float clampResult10_g9 = clamp( dotResult8_g9 , 0.0 , 1.0 );
				float3 temp_output_5_0_g10 = ( ( ase_worldPos - _target3 ) / _InfluenceRadius );
				float dotResult8_g10 = dot( temp_output_5_0_g10 , temp_output_5_0_g10 );
				float clampResult10_g10 = clamp( dotResult8_g10 , 0.0 , 1.0 );
				float3 temp_output_5_0_g12 = ( ( ase_worldPos - _target4 ) / _InfluenceRadius );
				float dotResult8_g12 = dot( temp_output_5_0_g12 , temp_output_5_0_g12 );
				float clampResult10_g12 = clamp( dotResult8_g12 , 0.0 , 1.0 );
				#ifdef _TARGETMODE_ON
				float staticSwitch161 = ( ( ( 1.0 - pow( clampResult10_g11 , 0.5 ) ) * _Activate_Target ) + ( ( 1.0 - pow( clampResult10_g9 , 0.5 ) ) * _Activate_Target_2 ) + ( _Activate_Target_3 * ( 1.0 - pow( clampResult10_g10 , 0.5 ) ) ) + ( _Activate_Target_4 * ( 1.0 - pow( clampResult10_g12 , 0.5 ) ) ) );
				#else
				float staticSwitch161 = 1.0;
				#endif
				float mask124 = ( tex2Dlod( _DisplacementMask, float4( panner4, 0, 0.0) ).r * staticSwitch161 );
				float temp_output_67_0 = step( 0.0 , 1.0 );
				float2 uv314 = v.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 uv415 = v.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				float4 appendResult17 = (float4(( uv314.x * -1.0 ) , uv314.y , uv415.x , 0.0));
				float4 temp_output_22_0 = ( float4( ( mask124 * v.ase_normal * ( _NormalPush * 0.01 ) * temp_output_67_0 ) , 0.0 ) + ( ( _DefaultShrink * appendResult17 * 0.01 ) + ( appendResult17 * _Shrink_Faces_Amplitude * mask124 * temp_output_67_0 * 0.01 ) ) );
				float2 uv4139 = v.ase_texcoord4.xy * float2( 1,1 ) + float2( 0,0 );
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = (( _ExtrudeUpFaces )?( ( temp_output_22_0 * uv4139.y ) ):( temp_output_22_0 )).xyz;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				o.clipPos = TransformObjectToHClip(v.vertex.xyz);
				return o;
			}

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				
				float Alpha = 1;
				float AlphaClipThreshold = 0.5;

				#if _AlphaClip
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}
			ENDHLSL
		}

	
	}
	CustomEditor "ShapeFxPackUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=17500
0;0;1920;1019;2325.493;-973.3879;1.350227;True;True
Node;AmplifyShaderEditor.CommentaryNode;26;-2203.424,-54.66446;Inherit;False;5180.465;991.6393;Comment;40;22;9;88;128;12;87;10;19;73;17;86;127;11;90;20;15;18;14;124;67;130;68;2;69;4;77;7;76;101;75;8;78;100;83;81;63;80;3;79;163;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;194;-1410.186,1029.934;Inherit;False;1827.973;1282.331;Influencer;24;182;113;178;164;148;179;176;165;153;154;177;166;189;180;192;187;186;184;193;183;168;162;161;191;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-2170.005,4.801289;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;79;-2151.865,317.4777;Float;False;Property;_TileX;TileX;15;0;Create;True;0;0;False;0;1;0;0.05;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;-2146.865,413.4777;Float;False;Property;_TileY;TileY;16;0;Create;True;0;0;False;0;1;0;0.05;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;63;-2163.262,145.7285;Inherit;False;1;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;83;-1876.584,58.87494;Float;False;Property;_Stretching;Stretching;17;0;Create;True;0;0;False;0;0;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;148;-1360.186,1352.681;Float;False;Property;_InfluenceRadius;InfluenceRadius;24;0;Create;True;0;0;False;0;0.5;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;81;-1847.863,264.4777;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector3Node;164;-1346.657,1478.971;Float;False;Property;_target2;target2;23;1;[HideInInspector];Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;113;-1327.142,1079.934;Float;False;Property;_target;target;22;1;[HideInInspector];Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;178;-1342.369,1743.824;Float;False;Property;_target3;target3;27;1;[HideInInspector];Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector3Node;182;-1350.345,2025.166;Float;False;Property;_target4;target4;28;1;[HideInInspector];Create;True;0;0;False;0;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.FunctionNode;179;-1085.658,2082.301;Inherit;False;SphereMask;-1;;12;988803ee12caf5f4690caee3c8c4a5bb;0;3;15;FLOAT3;0,0,0;False;14;FLOAT;5;False;12;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;78;-1586.033,62.03605;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;176;-1075.399,1833.811;Inherit;False;SphereMask;-1;;10;988803ee12caf5f4690caee3c8c4a5bb;0;3;15;FLOAT3;0,0,0;False;14;FLOAT;5;False;12;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.Matrix3X3Node;100;-1481.106,275.6328;Float;False;Constant;_Matrix0;Matrix 0;22;0;Create;True;0;0;False;0;0,1,0,1,0,0,0,0,0;0;1;FLOAT3x3;0
Node;AmplifyShaderEditor.FunctionNode;165;-1114.517,1563.984;Inherit;False;SphereMask;-1;;9;988803ee12caf5f4690caee3c8c4a5bb;0;3;15;FLOAT3;0,0,0;False;14;FLOAT;5;False;12;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;153;-1091.889,1200.756;Inherit;False;SphereMask;-1;;11;988803ee12caf5f4690caee3c8c4a5bb;0;3;15;FLOAT3;0,0,0;False;14;FLOAT;5;False;12;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;191;-671.1616,1934.382;Float;False;Property;_Activate_Target_3;Activate_Target_3;31;1;[HideInInspector];Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;187;-687.9996,2197.265;Float;False;Property;_Activate_Target_4;Activate_Target_4;30;1;[HideInInspector];Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;189;-635.1287,1677.702;Float;False;Property;_Activate_Target_2;Activate_Target_2;29;1;[HideInInspector];Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;192;-610.8397,1356.975;Float;False;Property;_Activate_Target;Activate_Target;32;1;[HideInInspector];Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;166;-721.905,1565.914;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1020.09,493.8668;Float;False;Property;_Animation_speed;Animation Speed;1;0;Create;False;0;0;False;0;1;0;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;101;-1170.936,148.7365;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3x3;0,0,0,1,1,1,1,0,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-1023.028,371.4201;Float;False;Property;_PannerY;PannerY;13;0;Create;True;0;0;False;0;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;180;-680.0136,2080.257;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-1022.028,278.4201;Float;False;Property;_PannerX;PannerX;14;0;Create;True;0;0;False;0;1;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;154;-513.9339,1200.185;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;177;-683.0095,1833.24;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;183;-346.9771,1566.566;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;193;-319.8627,1275.138;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;184;-466.6628,1808.882;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;77;-742.0276,269.4201;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;7;-712.0626,504.0772;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;163;-911.4342,59.50601;Float;False;Property;_DirectionChange;Direction Change;26;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;186;-460.772,2056.302;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;162;-80.90617,1514.952;Float;False;Constant;_Float3;Float 3;27;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;4;-571.3143,61.92897;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;168;-101.7416,1674.501;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;161;146.7869,1646.037;Float;False;Property;_TargetMode;TargetMode;25;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-315.0696,40.8592;Inherit;True;Property;_DisplacementMask;Mask Map;0;1;[NoScaleOffset];Create;False;0;0;False;0;-1;0fc8bf4d13e7b2c44872d87a42008190;0fc8bf4d13e7b2c44872d87a42008190;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BreakToComponentsNode;68;-1553.441,787.1879;Inherit;False;FLOAT;1;0;FLOAT;0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;69;-1558.588,857.5736;Float;False;Constant;_deformation_type_Factor;Transition Factor;13;0;Create;False;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;1115.004,214.2874;Inherit;False;3;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;130;251.1045,64.31826;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;1506.059,238.3456;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;67;-550.1345,810.4012;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;15;1131.64,592.7656;Inherit;False;4;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;124;1230.815,59.0615;Float;False;mask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;17;1709.121,389.137;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;20;1851.036,668.8928;Float;False;Property;_Shrink_Faces_Amplitude;Shrink Factor;3;0;Create;False;0;0;False;0;0;0;-2;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;73;1170.767,848.0627;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;1948.268,855.4913;Float;False;Constant;_Float1;Float 1;20;0;Create;True;0;0;False;0;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;86;2015.664,14.41169;Float;False;Property;_DefaultShrink;DefaultShrink;18;0;Create;True;0;0;False;0;0;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;298.8099,600.4836;Float;False;Property;_NormalPush;Normal Push;2;0;Create;False;0;0;False;0;0;0;-1;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;127;1647.042,702.643;Inherit;False;124;mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;695.8816,606.0494;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.01;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;2356.918,95.08242;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;2371.971,640.7537;Inherit;False;5;5;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.NormalVertexDataNode;10;452.9073,352.5459;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;128;595.5834,236.8195;Inherit;False;124;mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;860.2293,336.7623;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;88;2546.147,363.6688;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;139;2802.2,655.3958;Inherit;False;4;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;22;2779.594,324.3912;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.CommentaryNode;34;-2717.419,-1272.494;Inherit;False;828.1869;614.2216;MatcapUv;4;28;146;159;160;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;140;3051.768,497.6676;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector4Node;43;-561.5037,-1866.576;Float;False;Constant;_Vector1;Vector 1;9;0;Create;True;0;0;False;0;0.5,0.5,0.5,0.5;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;38;-257.5504,-2446.647;Float;False;Property;_Outline_Opacity;Outline Opacity;7;0;Create;False;0;0;False;0;1;0;0;200;0;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;92;-324.5876,-2282.434;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;-60.74072,-2830.661;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-126.4893,-1577.787;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;47;-398.1266,-1373.554;Float;False;Property;_FrontFace_Color;FrontFace Color;11;1;[HDR];Create;False;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;95;-457.7407,-2933.661;Float;False;Property;_DefaultOutlineOpacity;DefaultOutlineOpacity;19;0;Create;True;0;0;False;0;0;0;0;25;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-430.4011,-1469.032;Float;False;Property;_FrontFace_Intensity;Intensity Mult;9;0;Create;False;0;0;False;0;1;0;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;126;-257.3275,-2534.387;Inherit;False;124;mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TransformDirectionNode;159;-2428.9,-901.5688;Inherit;False;Object;View;False;Fast;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-267.07,-2709.459;Inherit;False;5;5;0;COLOR;0,0,0,0;False;1;COLOR;1,1,1,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;4;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DotProductOpNode;42;-293.6394,-2012.16;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;160;-2176.9,-856.5688;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;36;-1075.152,-2722.139;Inherit;True;Property;_OutlineTex;Outline Map;6;0;Create;False;0;0;False;0;-1;b23676ff9cac20a4c9c7b9333f055f1b;b23676ff9cac20a4c9c7b9333f055f1b;True;2;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CustomExpressionNode;146;-2180.284,-1052.073;Float;False;float2 uv_matcap = normal *0.5 + float2(0.5,0.5)@ float2(0.5,0.5)@$$return uv_matcap@;2;False;1;True;normal;FLOAT3;0,0,0;In;;Float;False;My Custom Expression;True;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;93;324.2593,-2832.661;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;717.688,-2310.317;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;52;736.6283,-885.4119;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.FaceVariableNode;61;1396.652,-974.2081;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;56;703.6519,-1581.069;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.NormalVertexDataNode;28;-2667.419,-1057.449;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;62;1563.623,-1223.438;Inherit;False;3;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;39;-603.7484,-2494.584;Float;False;Property;_Outline_Color;Outline Color;8;1;[HDR];Create;False;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;141;3294.109,323.7791;Float;False;Property;_ExtrudeUpFaces;ExtrudeUpFaces;21;0;Create;True;0;0;False;0;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector3Node;40;-592.7091,-2314.927;Float;False;Constant;_HDR_Factor;HDR_Factor;9;0;Create;True;0;0;False;0;0.3,0.3,0.3;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;49;-363.6474,-774.0141;Float;False;Property;_BackFace_Intensity;Intensity Mult;10;0;Create;False;0;0;False;0;1;0;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;51;153.3765,-881.1363;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ToggleSwitchNode;96;3316.306,-294.7485;Float;False;Property;_Debug_Mask;Debug_Mask;20;0;Create;True;0;0;False;0;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;57;124.3944,-1579.249;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;58;1062.358,-1580.74;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;50;-354.3729,-676.5362;Float;False;Property;_BackFace_Color;BackFace Color;12;1;[HDR];Create;False;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-59.73576,-881.7688;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;48.49939,-2708.229;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;27;-1657.885,-1563.108;Inherit;True;Property;_FrontFace_Diffuse_map;FrontFace_Diffuse_map;4;1;[NoScaleOffset];Create;True;0;0;False;0;-1;d8cfe409d2fb65842a7151f63c8307c5;d8cfe409d2fb65842a7151f63c8307c5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;125;2900.163,-277.2464;Inherit;False;124;mask;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;35;-1678.485,-894.098;Inherit;True;Property;_BackFace_Diffuse_map;BackFace Map;5;1;[NoScaleOffset];Create;False;0;0;False;0;-1;e6042d60a743b1145b5ea4a614f4aa98;e6042d60a743b1145b5ea4a614f4aa98;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;195;4107.055,42.30422;Float;False;True;-1;2;ShapeFxPackUI;0;3;Shapes_Shader_Pack;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;0;Forward;7;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;0;Hidden/InternalErrorShader;0;0;Standard;10;Surface;0;  Blend;0;Two Sided;1;Cast Shadows;1;Receive Shadows;1;GPU Instancing;1;LOD CrossFade;0;Built-in Fog;0;Meta Pass;0;Vertex Position,InvertActionOnDeselection;1;0;4;True;True;True;False;False;;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;196;4107.055,42.30422;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;1;ShadowCaster;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;197;4107.055,42.30422;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;2;DepthOnly;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;False;True;False;False;False;False;0;False;-1;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;198;4107.055,42.30422;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;1;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;3;Meta;0;False;False;False;True;0;False;-1;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;2;0;False;False;False;True;2;False;-1;False;False;False;False;False;True;1;LightMode=Meta;False;0;Hidden/InternalErrorShader;0;0;Standard;0;0
WireConnection;83;0;63;0
WireConnection;83;1;3;0
WireConnection;81;0;79;0
WireConnection;81;1;80;0
WireConnection;179;15;182;0
WireConnection;179;14;148;0
WireConnection;78;0;83;0
WireConnection;78;1;81;0
WireConnection;176;15;178;0
WireConnection;176;14;148;0
WireConnection;165;15;164;0
WireConnection;165;14;148;0
WireConnection;153;15;113;0
WireConnection;153;14;148;0
WireConnection;166;0;165;0
WireConnection;101;0;78;0
WireConnection;101;1;100;0
WireConnection;180;0;179;0
WireConnection;154;0;153;0
WireConnection;177;0;176;0
WireConnection;183;0;166;0
WireConnection;183;1;189;0
WireConnection;193;0;154;0
WireConnection;193;1;192;0
WireConnection;184;0;191;0
WireConnection;184;1;177;0
WireConnection;77;0;75;0
WireConnection;77;1;76;0
WireConnection;7;0;8;0
WireConnection;163;1;78;0
WireConnection;163;0;101;0
WireConnection;186;0;187;0
WireConnection;186;1;180;0
WireConnection;4;0;163;0
WireConnection;4;2;77;0
WireConnection;4;1;7;0
WireConnection;168;0;193;0
WireConnection;168;1;183;0
WireConnection;168;2;184;0
WireConnection;168;3;186;0
WireConnection;161;1;162;0
WireConnection;161;0;168;0
WireConnection;2;1;4;0
WireConnection;130;0;2;1
WireConnection;130;1;161;0
WireConnection;18;0;14;1
WireConnection;67;0;68;0
WireConnection;67;1;69;0
WireConnection;124;0;130;0
WireConnection;17;0;18;0
WireConnection;17;1;14;2
WireConnection;17;2;15;1
WireConnection;73;0;67;0
WireConnection;12;0;11;0
WireConnection;87;0;86;0
WireConnection;87;1;17;0
WireConnection;87;2;90;0
WireConnection;19;0;17;0
WireConnection;19;1;20;0
WireConnection;19;2;127;0
WireConnection;19;3;73;0
WireConnection;19;4;90;0
WireConnection;9;0;128;0
WireConnection;9;1;10;0
WireConnection;9;2;12;0
WireConnection;9;3;67;0
WireConnection;88;0;87;0
WireConnection;88;1;19;0
WireConnection;22;0;9;0
WireConnection;22;1;88;0
WireConnection;140;0;22;0
WireConnection;140;1;139;2
WireConnection;92;0;67;0
WireConnection;94;0;37;0
WireConnection;94;1;95;0
WireConnection;45;0;27;0
WireConnection;45;1;46;0
WireConnection;45;2;47;0
WireConnection;159;0;28;0
WireConnection;37;0;36;0
WireConnection;37;2;39;0
WireConnection;37;3;92;0
WireConnection;37;4;40;0
WireConnection;42;0;27;0
WireConnection;42;1;43;0
WireConnection;160;0;159;0
WireConnection;146;0;160;0
WireConnection;93;0;94;0
WireConnection;93;1;91;0
WireConnection;44;0;93;0
WireConnection;44;1;42;0
WireConnection;52;0;51;0
WireConnection;52;1;51;1
WireConnection;52;2;51;2
WireConnection;52;3;50;4
WireConnection;56;0;57;0
WireConnection;56;1;57;1
WireConnection;56;2;57;2
WireConnection;56;3;47;4
WireConnection;62;0;52;0
WireConnection;62;1;58;0
WireConnection;62;2;61;0
WireConnection;141;0;22;0
WireConnection;141;1;140;0
WireConnection;51;0;48;0
WireConnection;96;0;58;0
WireConnection;96;1;125;0
WireConnection;57;0;45;0
WireConnection;58;0;56;0
WireConnection;58;1;44;0
WireConnection;48;0;35;0
WireConnection;48;1;49;0
WireConnection;48;2;50;0
WireConnection;91;0;37;0
WireConnection;91;1;126;0
WireConnection;91;2;38;0
WireConnection;27;1;146;0
WireConnection;35;1;146;0
WireConnection;195;2;96;0
WireConnection;195;5;141;0
ASEEND*/
//CHKSM=096BDA6556E9295D854A4F83D206CA82A600C952