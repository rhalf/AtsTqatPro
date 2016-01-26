using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TqatProModel;

namespace TqatProViewModel {
    public class UserItem {

        User user;

        public UserItem(User user) {
            this.user = user;
        }
        public int Id {
            get {
                return user.Id;
            }
            set {
                user.Id = value;
            }
        } 
        public string Username {
            get {
                return user.Username;
            }
            set {
                user.Username = value;
            }
        }
        public bool IsChecked {
            get;
            set;
        }
        public User getUser() {
            return user;
        }
    }
}
