using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace YjkInspectClient
{
    /// <summary>
    /// 个人信息（打印记录）
    /// </summary>
    public class PeopleInfo : INotifyPropertyChanged
    {
        /// <summary>
        /// 身份证号码
        /// </summary>
        private string _cardId = string.Empty; 
        /// <summary>
        /// 姓名
        /// </summary>
        private string _name = string.Empty;
        /// <summary>
        /// 性别
        /// </summary>
        private string _sex = string.Empty;
        /// <summary>
        /// 年龄
        /// </summary>
        private string _age = string.Empty;
        /// <summary>
        /// 序号（流水号）
        /// </summary>
        private string _serialId = string.Empty;
        /// <summary>
        /// 打印时间
        /// </summary>
        private string _addTime = string.Empty;

        public string cardId
        {
            get { return _cardId; }
            set
            {
                if (string.IsNullOrEmpty(value) && value == _cardId)
                    return;
                _cardId = value;
                NotifyPropertyChanged(() => cardId);
            }
        }
        public string name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value) && value == _name)
                    return;
                _name = value;
                NotifyPropertyChanged(() => name);
            }
        }
        public string sex
        {
            get { return _sex; }
            set
            {
                if (string.IsNullOrEmpty(value) && value == _sex)
                    return;
                _sex = value;
                NotifyPropertyChanged(() => sex);
            }
        }
        public string age
        {
            get { return _age; }
            set
            {
                if (string.IsNullOrEmpty(value) && value == _age)
                    return;
                _age = value;
                NotifyPropertyChanged(() => age);
            }
        }
        public string serialId
        {
            get { return _serialId; }
            set
            {
                if (string.IsNullOrEmpty(value) && value == _serialId)
                    return;
                _serialId = value;
                NotifyPropertyChanged(() => serialId);
            }
        }
        public string addTime
        {
            get { return _addTime; }
            set
            {
                if (string.IsNullOrEmpty(value) && value == _addTime)
                    return;
                _addTime = value;
                NotifyPropertyChanged(() => addTime);
            }
        }

        public PeopleInfo() { }
        public PeopleInfo(String recordString)
        {
            string[] records = recordString.Split('\t');
            if (records.Length == 6)
            {
                _name = records[0];
                _cardId = records[1];
                _age = records[2];
                _sex = records[3];
                _serialId = records[4];
                _addTime = records[5];
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged<T>(Expression<Func<T>> property)
        {
            if (PropertyChanged == null)
                return;

            var memberExpression = property.Body as MemberExpression;
            if (memberExpression == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
        }

        public void clearInfo()
        {
            cardId = null;
            name = null;
            sex = null;
            age = null;
            addTime = null;
            //serialId = null;
        }

        public override string ToString()
        {
            return _name + "\t" + _cardId + "\t" + _age + "\t" + _sex + "\t" + _serialId + "\t" + _addTime;
        }

        public void copyTo(PeopleInfo people)
        {
            if (people == null)
            {
                people = new PeopleInfo();
            }
            people.addTime = _addTime;
            people.name = _name;
            people.cardId = _cardId;
            people.serialId = _serialId;
            people.sex = _sex;
            people.age = _age;
        }

        public PeopleInfo Clone()
        {
            return this.MemberwiseClone() as PeopleInfo;
        }
    }
}
