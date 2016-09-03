using UnityEngine;
using System.Threading;


class UnityCommands: AndroidJavaProxy {
    public int repaintCalled = 0;

    public UnityCommands(): base("com.github.listvin.testplugin.Commands") {
        Debug.Log("constructing UnityCommands...");
    }

    public void repaint() {
        Interlocked.Exchange(ref repaintCalled, 1);
        Debug.Log("Got repaint callback: " + repaintCalled);
    }
}
