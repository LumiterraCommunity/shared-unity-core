using System.Diagnostics;
using SharedCore.Editor;

public static class CommandTool
{

    public static void ProcessCommand(string command, string argument)
    {

        string processCmd = null;
        string processArgs = null;
        if (CapabilitiesTool.IsMac())
        {
            // 在mac上，启动进程，直接执行命令 出现无反应。
            // 这里采取间接方案，执行 shell.sh 文件，把命令传入其中，来进行命令执行。
            processCmd = Constant.BinSh;
            processArgs = $"{Constant.CommonHandleSh} {command} {argument}";
        }
        else
        {
            processCmd = command;
            processArgs = argument;
        }

        ProcessStartInfo info = new(processCmd);
        //启动应用程序时要使用的一组命令行参数。
        info.Arguments = processArgs;
        //是否弹窗
        info.CreateNoWindow = true;
        //获取或设置指示不能启动进程时是否向用户显示错误对话框的值。
        info.ErrorDialog = true;
        //获取或设置指示是否使用操作系统 shell 启动进程的值。
        info.UseShellExecute = false;

        if (info.UseShellExecute)
        {
            info.RedirectStandardOutput = false;
            info.RedirectStandardError = false;
            info.RedirectStandardInput = false;
        }
        else
        {
            info.RedirectStandardOutput = true; //获取或设置指示是否将应用程序的错误输出写入 StandardError 流中的值。
            info.RedirectStandardError = true; //获取或设置指示是否将应用程序的错误输出写入 StandardError 流中的值。
            info.RedirectStandardInput = true;//获取或设置指示应用程序的输入是否从 StandardInput 流中读取的值。
            info.StandardOutputEncoding = System.Text.Encoding.UTF8;
            info.StandardErrorEncoding = System.Text.Encoding.UTF8;
        }
        UnityEngine.Debug.Log($"执行命令: {command} {info.Arguments}");
        //启动(或重用)此 Process 组件的 StartInfo 属性指定的进程资源，并将其与该组件关联。
        Process process = Process.Start(info);
        //StandardInput：获取用于写入应用程序输入的流。
        //将字符数组写入文本流，后跟行终止符。
        // process.StandardInput.WriteLine(argument);
        //获取或设置一个值，该值指示 StreamWriter 在每次调用 Write(Char) 之后是否都将其缓冲区刷新到基础流。
        process.StandardInput.AutoFlush = true;

        string output = "";
        string outputError = "";
        if (!info.UseShellExecute)
        {
            output = process.StandardOutput.ReadToEnd();
            outputError = process.StandardError.ReadToEnd();
        }

        process.WaitForExit();
        //关闭
        process.Close();

        if (!string.IsNullOrEmpty(output))
        {
            UnityEngine.Debug.Log(output);
        }
        if (!string.IsNullOrEmpty(outputError))
        {
            throw new System.Exception(outputError);
        }
    }
}