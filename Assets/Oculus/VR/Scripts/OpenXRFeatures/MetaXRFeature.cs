#if USING_XR_SDK_OPENXR
using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Features;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.XR.OpenXR;
using UnityEditor.XR.OpenXR.Features;
using UnityEditor.Build.Reporting;
#endif

namespace Meta.XR
{
#if UNITY_EDITOR
    public class MetaXRFeatureEditorConfig
    {
        public const string OpenXrExtensionList =
            "XR_KHR_vulkan_enable " +
            "XR_KHR_D3D11_enable " +
            "XR_OCULUS_common_reference_spaces " +
            "XR_FB_display_refresh_rate " +
            "XR_EXT_performance_settings " +
            "XR_FB_composition_layer_image_layout " +
            "XR_KHR_android_surface_swapchain " +
            "XR_FB_android_surface_swapchain_create " +
            "XR_KHR_composition_layer_color_scale_bias " +
            "XR_FB_color_space " +
            "XR_EXT_hand_tracking " +
            "XR_FB_swapchain_update_state " +
            "XR_FB_swapchain_update_state_opengl_es " +
            "XR_FB_swapchain_update_state_vulkan " +
            "XR_FB_composition_layer_alpha_blend " +
            "XR_KHR_composition_layer_depth " +
            "XR_KHR_composition_layer_cylinder " +
            "XR_KHR_composition_layer_cube " +
            "XR_KHR_composition_layer_equirect2 " +
            "XR_KHR_convert_timespec_time " +
            "XR_KHR_visibility_mask " +
            "XR_FB_render_model " +
            "XR_FB_spatial_entity " +
            "XR_FB_spatial_entity_query " +
            "XR_FB_spatial_entity_storage " +
            "XR_META_performance_metrics " +
            "XR_FB_scene " +
            "XR_FB_spatial_entity_container " +
            "XR_FB_scene_capture " +
            "XR_FB_face_tracking " +
            "XR_FB_eye_tracking " +
            "XR_FB_keyboard_tracking " +
            "XR_FB_passthrough " +
            "XR_FB_triangle_mesh " +
            "XR_FB_passthrough_keyboard_hands " +
            "XR_OCULUS_audio_device_guid " +
            "XR_FB_common_events " +
            "XR_FB_space_warp " +
            "XR_FB_hand_tracking_capsules " +
            "XR_FB_hand_tracking_mesh " +
            "XR_FB_hand_tracking_aim " +
            "XR_FB_touch_controller_pro " +
            "XR_FB_touch_controller_proximity " +
            ""
            ;
    }
#endif

    /// <summary>
    /// MetaXR Feature for OpenXR
    /// </summary>
#if UNITY_EDITOR
    [OpenXRFeature(UiName = "Meta XR Feature",
        BuildTargetGroups = new[] { BuildTargetGroup.Standalone, BuildTargetGroup.Android },
        Company = "Meta",
        Desc = "Meta XR Feature for OpenXR.",
        DocumentationLink = "https://developer.oculus.com/",
        OpenxrExtensionStrings = MetaXRFeatureEditorConfig.OpenXrExtensionList,
        Version = "0.0.1",
        FeatureId = featureId)]
#endif
    public class MetaXRFeature : OpenXRFeature
    {
        /// <summary>
        /// The feature id string. This is used to give the feature a well known id for reference.
        /// </summary>
        public const string featureId = "com.meta.openxr.feature.metaxr";

        /// <inheritdoc />
        protected override IntPtr HookGetInstanceProcAddr(IntPtr func)
        {
            OVRPlugin.UnityOpenXR.Enabled = true;

            OVRPlugin.UnityOpenXR.SetClientVersion();

            return OVRPlugin.UnityOpenXR.HookGetInstanceProcAddr(func);
        }

        /// <inheritdoc />
        protected override bool OnInstanceCreate(ulong xrInstance)
        {
            bool isMetaHeadsetIdSupported = false;
            string[] extensions = OpenXRRuntime.GetAvailableExtensions();
            foreach (string extension in extensions)
            {
                if (extension == "XR_META_headset_id")
                {
                    isMetaHeadsetIdSupported = true;
                    break;
                }
            }

            if (isMetaHeadsetIdSupported)
            {
                
            }
            else
            {
                // The runtime name string will be used to support old runtime versions which misses XR_META_headset_id extension.
                // This path should be removed in the future.
                string runtimeNameLowercase = OpenXRRuntime.name.ToLower();
                if (!runtimeNameLowercase.Contains("meta") && !runtimeNameLowercase.Contains("oculus"))
                {
                    // disable MetaXRFeature from non-Oculus/Meta OpenXR runtimes
                    return false;
                }
            }

            // here's one way you can grab the instance
            Debug.Log($"[MetaXRFeature] OnInstanceCreate: {xrInstance}");
            bool result = OVRPlugin.UnityOpenXR.OnInstanceCreate(xrInstance);
            if (!result)
            {
                
            }
            return result;
        }

        /// <inheritdoc />
        protected override void OnInstanceDestroy(ulong xrInstance)
        {
            // here's one way you can grab the instance
            OVRPlugin.UnityOpenXR.OnInstanceDestroy(xrInstance);
        }

        /// <inheritdoc />
        protected override void OnSessionCreate(ulong xrSession)
        {
            // here's one way you can grab the session
            OVRPlugin.UnityOpenXR.OnSessionCreate(xrSession);
        }

        /// <inheritdoc />
        protected override void OnAppSpaceChange(ulong xrSpace)
        {
            OVRPlugin.UnityOpenXR.OnAppSpaceChange(xrSpace);
        }

        /// <inheritdoc />
        protected override void OnSessionStateChange(int oldState, int newState)
        {
            OVRPlugin.UnityOpenXR.OnSessionStateChange(oldState, newState);
        }

        /// <inheritdoc />
        protected override void OnSessionBegin(ulong xrSession)
        {
            OVRPlugin.UnityOpenXR.OnSessionBegin(xrSession);
        }

        /// <inheritdoc />
        protected override void OnSessionEnd(ulong xrSession)
        {
            OVRPlugin.UnityOpenXR.OnSessionEnd(xrSession);
        }

        /// <inheritdoc />
        protected override void OnSessionExiting(ulong xrSession)
        {
            OVRPlugin.UnityOpenXR.OnSessionExiting(xrSession);
        }

        /// <inheritdoc />
        protected override void OnSessionDestroy(ulong xrSession)
        {
            OVRPlugin.UnityOpenXR.OnSessionDestroy(xrSession);
        }

        // protected override void OnSessionLossPending(ulong xrSession) {}
        // protected override void OnInstanceLossPending (ulong xrInstance) {}
        // protected override void OnSystemChange(ulong xrSystem) {}
        // protected override void OnFormFactorChange (int xrFormFactor) {}
        // protected override void OnViewConfigurationTypeChange (int xrViewConfigurationType) {}
        // protected override void OnEnvironmentBlendModeChange (int xrEnvironmentBlendMode) {}
        // protected override void OnEnabledChange() {}
    }

#if UNITY_EDITOR && UNITY_OPENXR_BOOT_CONFIG
    internal class MetaXRBootConfig : OpenXRFeatureBuildHooks
    {
        public override int callbackOrder => 1;
        public override Type featureType => typeof(MetaXRFeature);

        protected override void OnPostGenerateGradleAndroidProjectExt(string path) {}
        protected override void OnPostprocessBuildExt(BuildReport report) {}
        protected override void OnPreprocessBuildExt(BuildReport report) {}

        protected override void OnProcessBootConfigExt(BuildReport report, BootConfigBuilder builder)
        {
            builder.SetBootConfigValue("xr-meta-enabled", "1");
        }
    }
#endif
}

#endif
