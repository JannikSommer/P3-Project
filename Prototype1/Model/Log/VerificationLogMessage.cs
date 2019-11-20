using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

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

        // TODO: Needs Verification. Possibly backingfields.
        public override DateTime Time { get; set; }
        public string ItemId { get; set; }
        public string UserId { get; set; }
        public bool IsVerified { get; set; }
        public override LogMessageType Type { get; }
        public override string Message { get; set; }

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
