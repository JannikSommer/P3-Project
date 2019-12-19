using System;

namespace Model.Log {
    public class VerificationLogMessage : LogMessage {
        public VerificationLogMessage(DateTime time, string userId, string itemId, bool isVerified) {
            Time = time;
            UserId = userId;
            ItemId = itemId;
            IsVerified = isVerified;
            Message = GetMessageString();
            Type = LogMessageType.Verification;
        }
        public VerificationLogMessage(string userId, string itemId, bool isVerified) : this(DateTime.Now, userId, itemId, isVerified) {}


        public bool IsVerified { get; set; }
        public string UserId {
            get {
                return _userId;
            }
            set {
                if(value != string.Empty) {
                    _userId = value;
                } else {
                    throw new ArgumentException();
                }
            }
        }
        public string ItemId {
            get {
                return _itemId;
            }
            set {
                if(value != string.Empty /* Check for specific itemId format */) {
                    _itemId = value;
                } else {
                    throw new ArgumentException();
                }
            }
        }
        public override string Message { get; set; }
        public override LogMessageType Type { get; }
        public override DateTime Time { get; set; }

        private string _userId;
        private string _itemId;


        public override string GetSaveString() {
            return Type.ToString()
                + _seperator
                + Time.ToString("s")
                + _seperator
                + UserId
                + _seperator
                + ItemId
                + _seperator
                + IsVerified.ToString();
        }

        public override string GetExportableString() {
            return Type.ToString()
                + _seperator
                + Time.ToString("s")
                + _seperator
                + UserId
                + GetMessageString();
        }

        public override string GetMessageString() {
            return UserId 
                + " has "
                + (IsVerified ? "verified" : "rejected")
                + " the counted quantity of "
                + ItemId;
        }
    }
}
