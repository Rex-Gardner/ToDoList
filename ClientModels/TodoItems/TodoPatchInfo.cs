using System.Runtime.Serialization;

namespace ClientModels.TodoItems
{
    [DataContract]
    public class TodoPatchInfo
    {
        [DataMember(IsRequired = false)]
        public string Title { get; set; }

        [DataMember(IsRequired = false)]
        public string Text { get; set; }
        
        [DataMember(IsRequired = false)]
        public string Priority { get; set; }
        
        [DataMember(IsRequired = false)]
        public string State { get; set; }
    }
}