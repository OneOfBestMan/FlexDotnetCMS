﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FrameworkLibrary
{
    public abstract class ChatRoom : IChatRoom
    {
        public Guid ChatRoomID { get; } = Guid.NewGuid();
        public string ChatRoomName { get; } = "";

        public RoomMode ChatRoomMode { get; } = RoomMode.Private;

        protected List<ChatUser> _currentUsers { get; set; } = new List<ChatUser>();
        private List<ChatMessage> _chatMessages = new List<ChatMessage>();

        public virtual void JoinChatRoom(ChatUser chatUser)
        {
            if (chatUser != null && !string.IsNullOrEmpty(chatUser.NickName))
            {
                _currentUsers.Add(chatUser);
                _chatMessages.Add(new ChatMessage(ref chatUser, $"{chatUser.NickName} joint the chat room", ChatMessageMode.System));
            }

        }

        public DateTime DateCreated { get; } = DateTime.Now;

        public ReadOnlyCollection<ChatUser> CurrentUsers
        {
            get
            {
                return _currentUsers.AsReadOnly();
            }
        }
        
        public ReadOnlyCollection<ChatMessage> ChatMessages
        {
            get
            {
                return _chatMessages.AsReadOnly();
            }
        }        

        public ChatRoom(string chatRoomName, List<ChatUser> currentUsers, RoomMode chatRoomMode)
        {
            _currentUsers = currentUsers;
            ChatRoomName = chatRoomName;
            ChatRoomMode = chatRoomMode;
        }

        public void AddMessage(ChatUser chatUser, string message)
        {
            var foundUser = _currentUsers.Find(i => i.SessionID == chatUser.SessionID);
            if (foundUser != null)
            {
                var newChatMessage = new ChatMessage(ref foundUser, message);
                _chatMessages.Add(newChatMessage);

                foundUser.LastChatMessageDateTime = newChatMessage.DateCreated;
            }
        }

        public void DeleteChatMessages()
        {
            _chatMessages = new List<ChatMessage>();
        }

        public ChatUser GetUserInChatRoom(ChatUser chatUser)
        {
            var foundUser = _currentUsers.Find(i => i.SessionID == chatUser.SessionID);

            if (foundUser != null)
                return foundUser;

            return null;
        }
    }
}