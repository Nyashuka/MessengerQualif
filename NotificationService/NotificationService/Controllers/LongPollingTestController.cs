using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;

namespace NotificationService.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class LongPollingTestController : ControllerBase
    {
        private static List<ChatMessage> messages = new List<ChatMessage>();
        private static List<WaitingClient> waitingClients = new List<WaitingClient>();


        // Отримання повідомлень (long polling)
        [HttpGet("poll")]
        public async Task<ActionResult<List<ChatMessage>>> Poll(string user)
        {
            var tcs = new TaskCompletionSource<List<ChatMessage>>();
            var waitingClient = new WaitingClient { User = user, TaskCompletionSource = tcs };

            lock (waitingClients)
            {
                waitingClients.Add(waitingClient);
            }

            var messages = await tcs.Task;
            return Ok(messages);
        }

        // Надсилання нового повідомлення
        [HttpPost("send")]
        public ActionResult Send(ChatMessage message)
        {
            lock (messages)
            {
                messages.Add(message);
            }

            // Повідомляємо всіх клієнтів, окрім відправника
            lock (waitingClients)
            {
                foreach (var waitingClient in waitingClients)
                {
                    // Перевірте, чи користувач не є відправником повідомлення
                    //if (waitingClient.User != message.User)
                    //{
                    //    waitingClient.TaskCompletionSource.SetResult(new List<ChatMessage> { message });
                    //}
                }
                waitingClients.RemoveAll(client => client.TaskCompletionSource.Task.IsCompleted);
            }

            return Ok();
        }
    }
}
