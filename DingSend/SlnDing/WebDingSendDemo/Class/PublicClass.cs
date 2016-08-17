using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DingDingInterface
{

    public class OperateJson
    {
        public T OptJson<T>(string strJson)
        {
            T _Ret = default(T);
            //反序列化
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(strJson));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            _Ret = (T)serializer.ReadObject(ms);
            return _Ret;
        }

        public string ToJson<T>(T value)
        {
            DataContractJsonSerializer dataContractSerializer = new DataContractJsonSerializer(value.GetType());
            MemoryStream ms = new MemoryStream();
            dataContractSerializer.WriteObject(ms, value);
            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }

        [DataContract]
    public class GetAccessTokenRet
    {
        [DataMember]
        public int errcode { get; set; }
        [DataMember]
        public string errmsg { get; set; }
        [DataMember]
        public string access_token { get; set; }
    }


    [DataContract]
    public class GetAccessTicketRet
    {
        [DataMember]
        public int errcode { get; set; }
        [DataMember]
        public string errmsg { get; set; }
        [DataMember]
        public string ticket { get; set; }
        [DataMember]
        public int expires_in { get; set; }

    }



    [DataContract]
    public class Dpment
    {
        [DataMember]
        public bool autoAddUser { get; set; }
        [DataMember]
        public bool createDeptGroup { get; set; }
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public int parentid { get; set; }       
    }

    [DataContract]
    public class GetDepartment
    {
        [DataMember]
        public List<Dpment> department { get; set; }
        [DataMember]
        public int errcode { get; set; }
        [DataMember]
        public string errmsg { get; set; }
    }

    [DataContract]
    public class GetDepartmentDetail
    {
        [DataMember]
        public int errcode { get; set; }// 返回码
        [DataMember]
        public string errmsg { get; set; }//对返回码的文本描述内容
        [DataMember]
        public int id { get; set; }// 部门id
        [DataMember]
        public string name { get; set; }//部门名称
        [DataMember]
        public int parentid { get; set; }//父部门id，根部门为1
        [DataMember]
        public int order { get; set; }// 在父部门中的次序值
        [DataMember]
        public bool createDeptGroup { get; set; }//是否同步创建一个关联此部门的企业群, true表示是, false表示不是
        [DataMember]
        public bool autoAddUser { get; set; }//当群已经创建后，是否有新人加入部门会自动加入该群, true表示是, false表示不是
        [DataMember]
        public bool deptHiding { get; set; }//  是否隐藏部门, true表示隐藏, false表示显示
        public string deptPerimits { get; set; }// 可以查看指定隐藏部门的其他部门列表，如果部门隐藏，则此值生效，取值为其他的部门id组成的的字符串，使用|符号进行分割
        [DataMember]
        public string userPerimits { get; set; }//可以查看指定隐藏部门的其他人员列表，如果部门隐藏，则此值生效，取值为其他的人员userid组成的的字符串，使用|符号进行分割
        [DataMember]
        public bool outerDept { get; set; }//   是否本部门的员工仅可见员工自己, 为true时，本部门员工默认只能看到员工自己
        [DataMember]
        public string outerPermitDepts { get; set; }//本部门的员工仅可见员工自己为true时，可以配置额外可见部门，值为部门id组成的的字符串，使用|符号进行分割
        [DataMember]
        public string outerPermitUsers { get; set; } //本部门的员工仅可见员工自己为true时，可以配置额外可见人员，值为userid组成的的字符串，使用| 符号进行分割
        [DataMember]
        public string orgDeptOwner { get; set; }//企业群群主
        [DataMember]
        public string deptManagerUseridList { get; set; }//部门的主管列表,取值为由主管的userid组成的字符串，不同的userid使用|符号进行分割

    }

    /*
       "errcode": 0,
    "errmsg": "ok",
    "hasMore": false,
    "userlist": [
        {
            "userid": "zhangsan",
            "name": "张三"
        }
    ]
     */
    public class Person
    {
        public string userid { get; set; }//员工唯一标识ID（不可修改）
        public string name { get; set; }//成员名称
    }

    [DataContract]
    public class DepartMember
    {
        [DataMember]
        public int errcode { get; set; }// 返回码
        [DataMember]
        public string errmsg { get; set; }//对返回码的文本描述内容
        [DataMember]
        public bool hasMore { get; set; }//在分页查询时返回，代表是否还有下一页更多数据
        [DataMember]
        public List<Person> userlist { get; set; }
    }

    [DataContract]
    public class PersonDetail
    {
        public int userid { get; set; }

        public string  dingId { get; set; }
        public string mobile { get; set; }
        public string tel { get; set; }
        public string workPlace { get; set; }
        public string remark { get; set; }
        public int order { get; set; }
        public bool isAdmin { get; set; }
        public bool isBoss { get; set; }
        public bool isHide { get; set; }
        public bool isLeader { get; set; }
        public string name { get; set; }
        public bool active { get; set; }
        public List<int> department { get; set; }
        public string position { get; set; }
        public string email { get; set; }
        public string avatar { get; set; }
        public string jobnumber { get; set; }
        public Dictionary<string,string> extattr { get; set; }
    }

    [DataContract]
    public class DepartMemberDetail
    {
        [DataMember]
        public int errcode { get; set; }// 返回码
        [DataMember]
        public string errmsg { get; set; }//对返回码的文本描述内容
        [DataMember]
        public bool hasMore { get; set; }//在分页查询时返回，代表是否还有下一页更多数据
        [DataMember]
        public List<PersonDetail> userlist { get; set; }
    }

    [DataContract]
    public class SendGeneralRet
    {
        [DataMember]
        public int errcode { get; set; }// 返回码
        [DataMember]
        public string errmsg { get; set; }//对返回码的文本描述内容
        [DataMember]
        public string receiver { get; set; }
    }

    [DataContract]
    public class CreateChat
    {
        public CreateChat()
        {
            useridlist = new List<string>();
        }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string owner { get; set; }
        [DataMember]
        public List<string> useridlist { get; set; }
    }

    [DataContract]
    public class CreateChatRet
    {
        [DataMember]
        public int errcode { get; set; }// 返回码
        [DataMember]
        public string errmsg { get; set; }//对返回码的文本描述内容
        [DataMember]
        public string chatid { get; set; }//
    }

    [DataContract]
    public class SendGroupChat
    {
        public SendGroupChat()
        {
            text = new SendString();  
        }
        [DataMember]
        public string chatid { get; set; }
        [DataMember]
        public string sender { get; set; }
        [DataMember]
        public string msgtype { get; set; }
        [DataMember]
        public SendString text { get; set; }
        [DataContract]
        public class SendString
        {
            [DataMember]
            public string content { get; set; }
        }

    }

    //{
    //"chatid": "chatxxxxxxxxx",
    //"sender": "manager1122",
    //"msgtype": "text",
    //"text": {
    //    "content": "张三的请假申请"
    //}


}
