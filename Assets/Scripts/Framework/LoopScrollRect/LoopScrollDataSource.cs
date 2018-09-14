using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
    public abstract class LoopScrollDataSource
    {
        public abstract void ProvideData(Transform transform, int idx);
    }

    public class LoopScrollSendIndexSource : LoopScrollDataSource
    {
        public static readonly LoopScrollSendIndexSource Instance = new LoopScrollSendIndexSource();

        LoopScrollSendIndexSource() { }

        public override void ProvideData(Transform transform, int idx)
        {
            transform.SendMessage("ScrollCellIndex", idx);
        }
    }

    public class LoopScrollArraySource<T> : LoopScrollDataSource
    {
        T[] objectsToFill;

        public LoopScrollArraySource(T[] objectsToFill)
        {
            this.objectsToFill = objectsToFill;
        }

        public override void ProvideData(Transform transform, int idx)
        {
            //transform.SendMessage("ScrollCellContent", objectsToFill[idx]);
            if (idx < 0 || idx >= objectsToFill.Length)
            {
                return;
            }
            var i = transform.GetComponent<ILoopScrollRectItem<T>>();
            if (i != null)
            {
                DebugFramework.Debugger.Log("i=" + i);
                i.SetData(objectsToFill[idx]);
            }
        }
    }
}