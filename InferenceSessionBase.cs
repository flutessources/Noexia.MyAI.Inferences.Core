using Noexia.MyAI.Inferences.Core.Logs;
using Noexia.MyAI.Models.Transport.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noexia.MyAI.Inferences.Core
{
    public abstract class InferenceSessionBase<TInput, TOutput, TUser>
        where TInput : IInferenceInput
        where TOutput : IInferenceOutput
        where TUser : IInferenceUser<TInput, TOutput>, new()
    {
        public const int QUEUE_UPDATE_INTERVAL_MS = 1000;
        public const int MAX_TIME_INPUT_SECONDS = 60;

        public bool IsOnInputProcess { get; protected set; } = false;
        public Action<string> onError;

        protected Queue<TUser> m_users = new Queue<TUser>();

        protected TUser m_currentUser;

        public abstract Task<bool> StartInference();

        protected abstract void InferenceInputProcess(TUser user);

        public virtual bool Start()
        {
            QueueUpdate(); // Started in a new thread
            return true;
        }

        /// <summary>
        /// Add user to queue, and wait for inference result
        /// </summary>
        /// <param name="input"></param>
        /// <param name="onResult"></param>
        /// <returns></returns>
        protected virtual TUser SendInput(TInput input, Action<TUser> beforeAddToqueue = null)
        {
            var user = new TUser();
            user.input = input;
            beforeAddToqueue?.Invoke(user);
            AddUserToQueue(user);
            return user;
        }

        protected virtual void AddUserToQueue(TUser user)
        {
            LogsManager.Log("Inference", "Add user to queue");
            m_users.Enqueue(user);
        }

        protected virtual void QueueUpdate()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (m_users.Count == 0 || CanSendInput() == false)
                    {
                        Logs.LogsManager.Log("Inference :", "No user in queue, wait for new user " + m_users.Count + " " + CanSendInput());
                        // No user in queue, wait for new user
                        // Use Thread.Sleep for minimize CPU usage
                        Thread.Sleep(QUEUE_UPDATE_INTERVAL_MS);
                        continue;
                    }

                    var user = m_users.Dequeue();
                    m_currentUser = user;

                    OnInferenceInputStart();

                    InferenceInputProcess(user);

                    var timeWatcher = new Stopwatch();
                    timeWatcher.Start();

                    while (IsOnInputProcess == true)
                    {
                        // Wait for inference input process to finish
                        // If timeout, stop inference input process
                        if (timeWatcher.ElapsedMilliseconds > MAX_TIME_INPUT_SECONDS * 1000)
                        {
                            Logs.LogsManager.Log("Inference :", "Inference input process timeout");
                            onError?.Invoke("Inference input process timeout");
                            break;
                        }
                    }

                    timeWatcher.Stop();

                }
            }, TaskCreationOptions.LongRunning);
        }

        protected virtual bool CanSendInput()
        {
            return true;
        }

        protected virtual void OnInferenceInputFinished()
        {
            IsOnInputProcess = false;
            Logs.LogsManager.Log("Inference :", "Inference input process totally finished");
        }

        protected virtual void OnInferenceInputStart()
        {
            IsOnInputProcess = true;
            Logs.LogsManager.Log("Inference :", "Start inference input");
        }

        // Not a transport based method, but a method to 
        protected void NotifyInferenceResult(TUser user, TOutput? result)
        {
            if (result == null)
            {
                onError?.Invoke("Inference result is null");
                return;
            }

            user?.onInferenceResult.Invoke(result);
        }
    }
}
