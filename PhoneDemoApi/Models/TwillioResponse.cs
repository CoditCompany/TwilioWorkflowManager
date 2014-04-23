//======================================================================================= 
// Codit - http://blog.codit.eu 
// 
// This sample contains the code of the Workflow Manager project that was demonstrated during 
// the BizTalk Summit on March 4, 2014, in London
// Please refer to my blog post at http://blog.codit.eu/post/2014/03/07/Using-a-State-Machine-in-Workflow-manager-to-instrument-a-Twilio-call.aspx/.  
//  
// Author: Sam Vanhoutte - @SamVanhoutte
//======================================================================================= 
// Copyright © 2014 Codit. All rights reserved. 
//  
// THIS CODE AND INFORMATION IS PROVIDED 'AS IS' WITHOUT WARRANTY OF ANY KIND, EITHER  
// EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF  
// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. YOU BEAR THE RISK OF USING IT. 
//======================================================================================= 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Serialization;

namespace PhoneDemoApi.Models
{
    //<Response>
    // <Say voice="alice" language="pt-BR" loop="2">Bom dia.</Say>
    //</Response>
    [XmlRoot("Response")]
    public class TwilioResponse
    {
        [XmlElement("Say")]
        public TwilioText SpokenText { get; set; }

        [XmlElement("Gather")]
        public TwilioCollect CollectInfo { get; set; }

        [XmlElement("Reject")]
        public TwilioReject Reject { get; set; }

        [XmlElement("Hangup")]
        public TwilioHangup Hangup { get; set; }
    }

    [XmlRoot("Hangup")]
    public class TwilioHangup
    {
    }


    [XmlRoot("Reject")]
    public class TwilioReject
    {
        [XmlAttribute("reason")]
        public string Reason { get; set; }
    }

    [XmlRoot("Say")]
    public class TwilioText
    {
        [XmlAttribute("voice")]
        public string Voice { get; set; }

        [XmlAttribute("language")]
        public string Language { get; set; }

        [XmlText()]
        public string Content { get; set; }
    }

    [XmlRoot("Play")]
    public class TwilioSound
    {
        [XmlText()]
        public string SoundUri { get; set; }
    }

    //<Gather timeout="10" finishOnKey="*">
    [XmlRoot("Gather")]
    public class TwilioCollect
    {
        private int _timeout = 15;

        [XmlAttribute("timeout")]
        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }
        
        [XmlAttribute("finishOnKey")]
        public string FinishingKey { get; set; }
        
        [XmlAttribute("action")]
        public string ActionUri { get; set; }
        
        [XmlAttribute("method")]
        public string ActionHttpMethod { get; set; }
        
        [XmlElement("Play")]
        public TwilioSound PlayedSound { get; set; }

        [XmlElement("Say")]
        public TwilioText SpokenText { get; set; }
    }
}
