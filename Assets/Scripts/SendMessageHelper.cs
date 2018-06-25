using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendMessageContext
{
    public GameObject Target;
    public string MethodName;
    public object Value;
    public SendMessageOptions Options = SendMessageOptions.RequireReceiver;

    public SendMessageContext(GameObject target, string methodName, object value, SendMessageOptions options)
    {
        this.Target = target;
        this.MethodName = methodName;
        this.Value = value;
        this.Options = options;
    }
}

public class SendMessageHelper : MonoBehaviour
{
    private static Queue<SendMessageContext> QueuedMessages = new Queue<SendMessageContext>();
    public static void RegisterSendMessage(SendMessageContext context)
    {
        lock (QueuedMessages)
        {
            QueuedMessages.Enqueue(context);
        }
    }

    private void Update()
    {
        while (QueuedMessages.Count > 0)
        {
            SendMessageContext context = null;
            lock (QueuedMessages)
            {
                context = QueuedMessages.Dequeue();
            }

            context.Target.SendMessage(context.MethodName, context.Value, context.Options);
        }
    }
}