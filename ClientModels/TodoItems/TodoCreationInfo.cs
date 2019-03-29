using System.Runtime.Serialization;

namespace ClientModels.TodoItems
{
    [DataContract]
    public class TodoCreationInfo
    {
        [DataMember(IsRequired = true)]
        public string Title { get; set; }

        [DataMember(IsRequired = true)]
        public string Text { get; set; }
        
        [DataMember(IsRequired = false)]
        public string Priority { get; set; }
    }
}
