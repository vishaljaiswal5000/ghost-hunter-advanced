using System.IO;
using UnityEditor;
using UnityEngine;

// Build > WebGL (itch.io): builds into Builds/WebGL, ready to zip and upload to itch.io
public static class BuildWebGL
{
    const string OutputDir = "Builds/WebGL";

    [MenuItem("Build/WebGL (itch.io)")]
    public static void Build()
    {
        // uncompressed output works on itch.io without any server header setup
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        PlayerSettings.WebGL.decompressionFallback = false;
        PlayerSettings.WebGL.nameFilesAsHashes = true;

        string[] scenes = System.Array.FindAll(
            System.Array.ConvertAll(EditorBuildSettings.scenes, s => s.enabled ? s.path : null),
            p => p != null);

        var options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = OutputDir,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        var report = BuildPipeline.BuildPlayer(options);
        Debug.Log("WebGL build " + report.summary.result + " -> " + Path.GetFullPath(OutputDir) + " (" + report.summary.totalSize / 1024 + " KB)");
        if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            throw new System.Exception("WebGL build failed");
        }
    }
}
