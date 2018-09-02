using UnityEngine;
using System.Collections;
public class TestConfig : IConfig
{
    //对应数据库中列名为ID的数据
    [ConfigField("ID")]
    public string m_id { get; set; }

    [ConfigField("Name")]
    public string m_name { get; set; }

    [ConfigField("Len")]
    public int m_len { get; set; }

    public override string ToString()
    {
        return string.Format("Id : {0}, Name : {1}, Len : {2}", m_id, m_name, m_len);
    }
}