using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using UnityEditor;

using System;

using System.IO;

public class APPBuildEditor : Editor

{

    [MenuItem("Tools/构建Apk")]

    public static void BuildApk()

    {

        //覆盖拷贝工程Lua相关文件，到 StreamingAssets文件夹内 
        /*
        string projectPath = Path.GetDirectoryName(Application.dataPath).Replace("\\", "/");

        string luaPath = projectPath + "/Lua";

        string outPath = projectPath + "/Assets/StreamingAssets/Lua";

        CopyDir(luaPath, outPath);
        */
        //生成apk 

        string apksPath = Path.GetDirectoryName(Application.dataPath) + "/Apks";

        if (!Directory.Exists(apksPath))

        {

            Directory.CreateDirectory(apksPath);

        }


        EditorUserBuildSettings.development = true;

        MobileTextureSubtarget originalSetting = EditorUserBuildSettings.androidBuildSubtarget;

        EditorUserBuildSettings.androidBuildSubtarget = originalSetting;

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions(); ;

        //Scene0
        buildPlayerOptions.scenes = new string[] { "Assets/Scenes/Menu.unity", "Assets/Scenes/Scene0.unity", "Assets/Scenes/Scene1.unity", "Assets/Scenes/Scene2.unity" };

        buildPlayerOptions.target = BuildTarget.Android;

        buildPlayerOptions.targetGroup = BuildTargetGroup.Android;

        buildPlayerOptions.options = BuildOptions.None;

        string localPathName = string.Format("{0}/{1}_{2}.apk", apksPath, "apk", DateTime.Now.ToString("yyyy_MM_dd_HH.mm.ss"));

        buildPlayerOptions.locationPathName = localPathName;

        BuildPipeline.BuildPlayer(buildPlayerOptions);

    }

    /// <summary> 

    /// 复制文件夹所有文件 

    /// </summary> 

    /// <param name="sourcePath">源目录</param> 

    /// <param name="destPath">目的目录</param> 

    public static void CopyFolder(string sourcePath, string destPath)

    {

        GetFiles(sourcePath, destPath, (sPath, cPath) =>

        {

            File.Copy(sPath, cPath, true);

        });

    }

    /// <summary> 

    /// 获取所有目录文件 

    /// </summary> 

    /// <param name="sourcePath"></param> 

    /// <param name="destPath"></param> 

    /// <param name="call"></param> 

    public static void GetFiles(string sourcePath, string destPath, Action<string, string> call)

    {

        if (Directory.Exists(sourcePath))

        {

            if (!Directory.Exists(destPath))

            {

                try

                {

                    Directory.CreateDirectory(destPath);

                }

                catch (Exception ex)

                {

                    throw new Exception("创建目标目录失败：" + ex.Message);

                }

            }

            List<string> files = new List<string>(Directory.GetFiles(sourcePath));

            files.ForEach(c =>

            {

                string destFile = Path.Combine(new string[] { destPath, Path.GetFileName(c) });

                call?.Invoke(c, destFile);

            });

        }

    }

    public static void CopyDir(string srcPath, string aimPath)

    {

        try

        {

            // 检查目标目录是否以目录分割字符结束如果不是则添加 

            if (aimPath[aimPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)

            {

                aimPath += System.IO.Path.DirectorySeparatorChar;

            }

            // 判断目标目录是否存在如果不存在则新建 

            if (!System.IO.Directory.Exists(aimPath))

            {

                System.IO.Directory.CreateDirectory(aimPath);

            }

            string[] fileList = System.IO.Directory.GetFileSystemEntries(srcPath);

            // 遍历所有的文件和目录 

            foreach (string file in fileList)

            {

                // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件 

                if (System.IO.Directory.Exists(file))

                {

                    CopyDir(file, aimPath + System.IO.Path.GetFileName(file));

                }

                // 否则直接Copy文件 

                else

                {

                    if (file.Contains(".lua"))

                    {

                        System.IO.File.Copy(file, aimPath + System.IO.Path.GetFileName(file), true);

                    }

                }

            }

        }

        catch (Exception e)

        {

            throw;

        }

    }

}