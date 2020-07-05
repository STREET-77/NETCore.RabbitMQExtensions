using System;
namespace NETCore.RabbitMQExtensions.Service
{
    internal delegate void AckMessageHandler();
    internal delegate void NackMessageHandler(bool requeue);

    public class ConsumerEventMessage
    {
        public byte[] Message { get; set; }

        internal event AckMessageHandler OnBasicAck;
        internal event NackMessageHandler OnBasicNack;

        /// <summary>
        /// 手动确认消息
        /// </summary>
        public void BasicAck() => OnBasicAck();

        /// <summary>
        /// 消息拒绝重回队列时，需注意数据的幂等性
        /// </summary>
        /// <param name="requeue"></param>
        public void BasicNack(bool requeue = false) => OnBasicNack(requeue);
    }
}
