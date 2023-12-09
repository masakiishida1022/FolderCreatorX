using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System;

using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        try
        {
            // CSVファイルのパスを指定
            string csvFilePath = @"C:\Temp\成果アクション\users.csv";
            string[] lines = File.ReadAllLines(csvFilePath);

            foreach (string line in lines)
            {
                string[] usernames = line.Split(',');   
                // 二つのユーザ名を連結してフォルダ名を作成
                string folderName = $"{usernames[0]}({usernames[1]})";

                // フォルダのパスを指定
                string folderPath = Path.Combine(@"C:\Temp\成果アクション\", folderName);

                // フォルダを作成
                CreateFolder(folderPath);

                // 最初のユーザに権限を付与    
                GrantFolderAccess(folderPath, usernames[2]);

                Console.WriteLine("処理が正常に完了しました。");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"エラー: {ex.Message}");
        }

        Console.ReadLine();
    }

  

    static void CreateFolder(string path)
    {
        // フォルダが存在しない場合は作成
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Console.WriteLine($"フォルダ '{path}' を作成しました。");
        }
        else
        {
            Console.WriteLine($"フォルダ '{path}' は既に存在します。");
        }
    }

    static void GrantFolderAccess(string path, string username)
    {
        try
        {
            // フォルダの権限を設定（読み取りと書き込み権限を所有者にのみ付与）
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            directoryInfo.Create();
            directoryInfo.Refresh();
            DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();
            directorySecurity.AddAccessRule(new FileSystemAccessRule(username,
                FileSystemRights.Read | FileSystemRights.Write,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
                PropagationFlags.None,
                AccessControlType.Allow));

            // 設定を反映
            directoryInfo.SetAccessControl(directorySecurity);

            Console.WriteLine($"ユーザ '{username}' に読み取りと書き込み権限を付与しました。");
        }
        catch (Exception ex)
        {
            throw new Exception($"権限の設定中にエラーが発生しました: {ex.Message}");
        }
    }
}