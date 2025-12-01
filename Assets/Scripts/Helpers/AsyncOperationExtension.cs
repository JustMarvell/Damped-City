using System.Threading.Tasks;
using UnityEngine;

public static class AsyncOperationExtension
{
    public static Task AsTask(this AsyncOperation asyncOp)
    {
        var tcs = new TaskCompletionSource<bool>();
        asyncOp.completed += _ => tcs.SetResult(true);
        return tcs.Task;
    }
}