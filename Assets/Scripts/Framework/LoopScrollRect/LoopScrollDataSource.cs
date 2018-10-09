using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
    public abstract class LoopScrollDataSource
    {
        public abstract void ProvideData(Transform transform, int idx);
        public abstract void ClearData(Transform transform);
    }

    public class LoopScrollSendIndexSource : LoopScrollDataSource
    {
        public static readonly LoopScrollSendIndexSource Instance = new LoopScrollSendIndexSource();

        LoopScrollSendIndexSource() { }

        public override void ProvideData(Transform transform, int idx)
        {
            transform.SendMessage("ScrollCellIndex", idx);
        }

        public override void ClearData(Transform transform)
        {

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
                Debugger.Log("i=" + i);
                i.SetData(objectsToFill[idx]);
            }
        }

        public override void ClearData(Transform transform)
        {
            var i = transform.GetComponent<ILoopScrollRectItem<T>>();
            if (i != null)
            {
                Debugger.Log("ClearData i=" + i);
                i.ClearData();
            }
        }
    }
}