﻿using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketRoom.Models.Custom;
using TicketRoom.Views.MainTab.Dael.Purchase;
using TicketRoom.Views.MainTab.Shop;
using TicketRoom.Views.Users.CreateUser;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TicketRoom.Views.MainTab.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupTermsList
    {
        public string input_string = "";
        public bool is_input = false;
        public PopupTermsList()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            TermsListGrid.Children[0].GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    string content = "제 1 조 (목적)\n\n이 약관은 비긴(이하 \"회사\"라 합니다)이 제공하는 티켓룸 서비스(이하 \"서비스\"라 합니다)와 관련하여, 회사와 이용 고객간에 서비스의 이용조건 및 절차, 회사와 회원간의 권리, 의무 및 기타 필요한 사항을 규정함을 목적으로 합니다. 본 약관은 PC통신, 스마트폰(안드로이드폰, 아이폰 등) 앱 등을 이용하는 전자상거래에 대해서도 그 성질에 반하지 않는 한 준용됩니다. \n\n제 2 조 (용어의 정의)\n\n1. \"사이트\"란 \"업주\"가 재화 또는 서비스 상품(이하 \"재화 등\"이라 합니다)을 \"이용자\"에게 판매하기 위하여, \"회사\"가 컴퓨터 등 정보통신설비를 이용하여 재화 등을 거래할 수 있도록 설정하여 제공하는 가상의 영업장을 말합니다. \n2. \"회원\"이라 함은 \"티켓룸\"에 개인정보를 제공하여 회원등록을 한 자로서, \"티켓룸\"의 정보를 지속적으로 제공받으며, \"티켓룸\"이 제공하는 서비스를 계속적으로 이용할 수 있는 자를 의미하고, \"티켓룸\" 광고업소는 포함되지 않습니다. \n3. \"비회원\"이라 함은 \"회원\"으로 가입하지 않고 \"회사\"가 제공하는 서비스를 이용하는 자를 말합니다. \n4. \"이용자\"라 함은 티켓룸 서비스를 이용하는 자를 말하며, 회원과 비회원을 모두 포함합니다. \n5. \"비밀번호(Password)\"라 함은 회원의 동일성 확인과 회원의 권익 및 비밀보호를 위하여 회원 스스로가 설정하여 사이트에 등록한 영문과 숫자 등의 조합을 말합니다. \n6. \"게시물\"이라 함은 \"회원\"이 서비스를 이용함에 있어 서비스 상에 게시한 부호ㆍ문자ㆍ음성ㆍ음향ㆍ화상ㆍ동영상 등의 정보 형태의 글, 사진, 동영상 및 각종 파일과 링크 등을 의미합니다. \n7. \"바로결제서비스\"라 함은 \"회원\"이 \"업주\"의 \"재화 등\"을 주문, 결제할 수 있도록 \"회사\"가 제공하는 서비스를 말하며, 결제방식은 바로결제방식과 만나서결제방식으로 나누어집니다. \n8. \"업주\"란 \"회사\"가 제공하는 \"서비스\"를 이용해 \"재화 등\"에 관한 정보를 게재하고, 판매(조리 및 배달포함)하는 주체를 말합니다. \n\n제 3 조 (약관의 명시와 개정)\n\n1. \"회사\"는 이 약관의 내용과 상호, 영업소 소재지 주소(소비자의 불만을 처리할 수 있는 곳의 주소를 포함), 대표자의 성명, 사업자등록번호, 통신판매업 신고번호, 연락처(전화, 전자우편 주소 등) 등을 \"이용자\"가 쉽게 알 수 있도록 \"사이트\"의 초기 서비스화면(전면)에 게시합니다. 다만, 약관의 내용은 \"이용자\"가 연결화면을 통하여 보도록 할 수 있습니다. \n2. \"회사\"는 『전자상거래 등에서의 소비자보호에 관한 법률』, 『약관의 규제등에 관한 법률』, 『전자문서 및 전자거래기본법』, 『전자서명법』, 『정보통신망 이용촉진 등에 관한 법률』, 『소비자기본법』 등 관련법령을 위배하지 않는 범위에서 이 약관을 개정할 수 있습니다. \n3. \"회사\"는 약관을 개정할 경우에는 적용일자 및 개정사유를 명시하여 현행약관과 함께 \"사이트\"의 초기화면에 그 적용일자 7일 이전부터 적용일자 전일까지 공지합니다. 다만, \"이용자\"에게 불리하게 약관내용을 변경하는 경우에는 최소한 30일 이상의 사전 유예기간을 두고 공지합니다. 이 경우 \"회사\"는 개정 전과 개정 후 내용을 \"이용자\"가 알기 쉽도록 표시합니다. \n4. \"회원\"은 변경된 약관에 동의하지 않을 권리가 있으며, 변경된 약관에 동의하지 않을 경우에는 서비스 이용을 중단하고 탈퇴를 요청할 수 있습니다. 다만, \"이용자\"가 제3항의 방법 등으로 \"회사\"가 별도 고지한 약관 개정 공지 기간 내에 \"회사\"에 개정 약관에 동의하지 않는다는 명시적인 의사표시를 하지 않는 경우 변경된 약관에 동의한 것으로 간주합니다. \n5. 이 약관에서 정하지 아니한 사항과 이 약관의 해석에 관하여는 『전자상거래 등에서의 소비자보호에 관한 법률』, 『약관의 규제 등에 관한 법률』, 공정거래위원회가 정하는 『전자상거래 등에서의 소비자보호지침』 및 관계 법령 또는 상관례에 따릅니다.\n\n제 4 조 (관련법령과의 관계)\n\n이 약관 또는 개별약관에서 정하지 않은 사항은 전기통신사업법, 전자거래기본법, 정보통신망법, 전자상거래 등에서의 소비자보호에 관한 법률, 개인정보보호법 등 관련 법령의 규정과 일반적인 상관례에 의합니다. \n\n제 5 조 (서비스의 제공 및 변경)\n\n1. \"회사\"는 다음과 같은 서비스를 제공합니다. \n1) \"재화 등\"에 대한 광고플랫폼 서비스 \n2) \"재화 등\"에 대한 주문중계 등 통신판매중개서비스 \n3) \"재화 등\"에 대한 판매 서비스 \n4) 배달대행 서비스 \n5) 위치기반 서비스 \n6) 기타 \"회사\"가 정하는 서비스 \n2. \"회사\"는 서비스 제공과 관련한 회사 정책의 변경 등 기타 상당한 이유가 있는 경우 등 운영상, 기술상의 필요에 따라 제공하고 있는 \"서비스\"의 전부 또는 일부를 변경 또는 중단할 수 있습니다. \n3. \"서비스\"의 내용, 이용방법, 이용시간에 대하여 변경 또는 \"서비스\" 중단이 있는 경우에는 변경 또는 중단될 \"서비스\"의 내용 및 사유와 일자 등은 그 변경 또는 중단 전에 회사 \"웹사이트\" 또는 \"서비스\" 내 \"공지사항\" 화면 등 \"회원\"이 충분이 인지할 수 있는 방법으로 사전에 공지합니다. \n\n제 6 조 (서비스 이용시간 및 중단)\n\n1. \"서비스\"의 이용은 \"회사\"의 업무상 또는 기술상 특별한 지장이 없는 한 연중무휴 1일 24시간을 원칙으로 합니다. 다만, 정기 점검 등의 필요로 \"회사\"가 정한 날이나 시간은 제외됩니다. 정기점검시간은 서비스제공화면에 공지한 바에 따릅니다. \n2. \"회사\"는 \"서비스\"의 원활한 수행을 위하여 필요한 기간을 정하여 사전에 공지하고 서비스를 중지할 수 있습니다. 단, 불가피하게 긴급한 조치를 필요로 하는 경우 사후에 통지할 수 있습니다. \n3. \"회사\"는 컴퓨터 등 정보통신설비의 보수점검•교체 및 고장, 통신의 두절 등의 사유가 발생한 경우에는 \"서비스\"의 제공을 일시적으로 중단할 수 있습니다. \n\n제 7 조 (이용계약의 성립)\n\n1. 이용계약은 \"회원\"이 되고자 하는 자(이하 \"가입신청자\")가 약관의 내용에 동의를 하고, \"회사\"가 정한 가입 양식에 따라 회원정보를 기입하여 회원가입신청을 하고 \"회사\"가 이러한 신청에 대하여 승인함으로써 체결됩니다. \n2. \"회사\"는 다음 각 호에 해당하는 신청에 대하여는 승인을 하지 않거나 사후에 이용계약을 해지할 수 있습니다. \n1) 가입신청자가 이 약관에 의하여 이전에 회원자격을 상실한 적이 있는 경우. 다만, 회원자격 상실 후 3년이 경과한 자로서 회사의 회원 재가입 승낙을 얻은 경우에는 예외로 함 \n2) 실명이 아니거나 타인의 명의를 이용한 경우 \n3) 회사가 실명확인절차를 실시할 경우에 이용자의 실명가입신청이 사실 아님이 확인된 경우 \n4) 등록내용에 허위의 정보를 기재하거나, 기재누락, 오기가 있는 경우 \n5) 이미 가입된 회원과 전화번호나 전자우편주소가 동일한 경우 \n7) 부정한 용도 또는 영리를 추구할 목적으로 본 서비스를 이용하고자 하는 경우 \n8) 기타 이 약관에 위배되거나 위법 또는 부당한 이용신청임이 확인된 경우 및 회사가 합리적인 판단에 의하여 필요하다고 인정하는 경우 \n3. 제1항에 따른 신청에 있어 \"회사\"는 \"회원\"에게 전문기관을 통한 실명확인 및 본인인증을 요청할 수 있습니다. \n4. \"회사\"는 서비스관련설비의 여유가 없거나, 기술상 또는 업무상 문제가 있는 경우에는 승낙을 유보할 수 있습니다. \n5. \"회원\"은 회원가입 시 등록한 사항에 변경이 있는 경우, 상당한 기간 이내에 \"회사\"에 대하여 회원정보 수정 등의 방법으로 그 변경사항을 알려야 합니다. \n\n제 8 조 (이용계약의 종료)\n\n1. \"회원\"의 해지 \n1) \"회원\"은 언제든지 \"회사\"에게 해지의사를 통지함으로써 이용계약을 해지할 수 있습니다. \n2) \"회사\"는 전항에 따른 \"회원\"의 해지요청에 대해 특별한 사정이 없는 한 이를 즉시 처리합니다. \n3) \"회원\"이 계약을 해지하는 경우 \"회원\"이 작성한 게시물은 삭제되지 않습니다. \n2. \"회사\"의 해지 \n1) \"회사\"는 다음과 같은 사유가 있는 경우, 이용계약을 해지할 수 있습니다. 이 경우 \"회사\"는 \"회원\"에게 전자우편, 전화, 팩스 기타의 방법을 통하여 해지사유를 밝혀 해지의사를 통지합니다. 다만, \"회사\"는 해당 \"회원\"에게 해지사유에 대한 의견진술의 기회를 부여 할 수 있습니다. \n가. 제7조 제2항에서 정하고 있는 이용계약의 승낙거부사유가 있음이 확인된 경우 \n나. \"회원\"이 \"회사\"나 다른 회원 기타 타인의 권리나 명예, 신용 기타 정당한 이익을 침해하는 행위를 한 경우 \n다. 기타 \"회원\"이 이 약관 및 \"회사\"의 정책에 위배되는 행위를 하거나 이 약관에서 정한 해지사유가 발생한 경우 \n라. 1년 이상 서비스를 이용한 이력이 없는 경우 \n2) 이용계약은 \"회사\"가 해지의사를 \"회원\"에게 통지함으로써 종료됩니다. 이 경우 \"회사\"가 해지의사를 \"회원\"이 등록한 전자우편주소로 발송하거나 \"회사\" 게시판에 게시함으로써 통지에 갈음합니다. \n\n제 9 조 (회원의 ID 및 비밀번호에 대한 의무)\n\n1. ID와 비밀번호에 관한 관리책임은 \"회원\"에게 있습니다. \n2. \"회원\"은 자신의 ID 및 비밀번호를 제3자에게 이용하게 해서는 안됩니다. \n3. \"회원\"이 자신의 ID 및 비밀번호를 도난 당하거나 제3자가 사용하고 있음을 인지한 경우에는 즉시 \"회사\"에 통보하고 \"회사\"의 조치가 있는 경우에는 그에 따라야 합니다. \n4. \"회원\"이 제3항에 따른 통지를 하지 않거나 \"회사\"의 조치에 응하지 아니하여 발생하는 모든 불이익에 대한 책임은 \"회원\"에게 있습니다. \n\n제 10 조 (회원 및 이용자의 의무)\n\n1. \"이용자\"는 관계법령 및 이 약관의 규정, \"회사\"의 정책, 이용안내 등 \"회사\"가 통지 또는 공지하는 사항을 준수하여야 하며, 기타 \"회사\" 업무에 방해되는 행위를 하여서는 안됩니다. \n2. \"이용자\"는 서비스 이용과 관련하여 다음 각 호의 행위를 하여서는 안됩니다. \n1) 서비스 신청 또는 변경 시 허위내용의 등록 \n2) \"회사\"에 게시된 정보의 허가 받지 않은 변경 \n3) \"회사\"가 정한 정보 이외의 정보(컴퓨터 프로그램 등)의 송신 또는 게시 \n4) \"회사\" 또는 제3자의 저작권 등 지적 재산권에 대한 침해 \n5) \"회사 또는 제3자의 명예를 손상시키거나 업무를 방해하는 행위 \n6) 외설 또는 폭력적인 메시지, 화상, 음성 기타 공공질서 미풍양속에 반하는 정보를 \"서비스\"에 공개 또는 게시하는 행위 \n7) 고객센터 상담 내용이 욕설, 폭언, 성희롱 등에 해당하는 행위 \n8) 포인트를 부정하게 적립하거나 사용하는 등의 행위 \n9) 허위 주문, 허위 리뷰작성 등을 통해 서비스를 부정한 목적으로 이용하는 행위 \n10) 자신의 ID, PW를 제3자에게 양도하거나 대여하는 등의 행위 \n11) 정당한 사유 없이 당사의 영업을 방해하는 내용을 기재하는 행위 \n12) 리버스엔지니어링, 디컴파일, 디스어셈블 및 기타 일체의 가공행위를 통하여 서비스를 복제, 분해 또 모방 기타 변형하는 행위 \n13) 자동 접속 프로그램 등을 사용하는 등 정상적인 용법과 다른 방법으로 서비스를 이용하여 회사의 서버에 부하를 일으켜 회사의 정상적인 서비스를 방해하는 행위 \n14) 기타 관계법령에 위반된다고 판단되는 행위 \n3. \"회사\"는 이용자가 본 조 제2항의 금지행위를 한 경우 본 약관 제13조에 따라 서비스 이용 제한 조치를 취할 수 있습니다. \n\n제 11 조 (회원의 게시물)\n\n\"회원\"이 작성한 게시물에 대한 저작권 및 모든 책임은 이를 게시한 \"회원\"에게 있습니다. 단, \"회사\"는 \"회원\"이 게시하거나 등록하는 게시물의 내용이 다음 각 호에 해당한다고 판단되는 경우 해당 게시물을 사전통지 없이 삭제 또는 임시조치(블라인드)할 수 있습니다. \n1) 다른 회원 또는 제3자를 비방하거나 명예를 손상시키는 내용인 경우 \n2) 공공질서 및 미풍양속에 위반되는 내용일 경우 \n3) 범죄적 행위에 결부된다고 인정되는 경우 \n4) 회사의 저작권, 제3자의 저작권 등 기타 권리를 침해하는 내용인 경우 \n5) 회원이 사이트와 게시판에 음란물을 게재하거나 음란사이트를 링크하는 경우 \n6) 회사로부터 사전 승인 받지 아니한 상업광고, 판촉 내용을 게시하는 경우 \n7) 해당 상품과 관련 없는 내용인 경우 \n8) 정당한 사유 없이 \"회사\" 또는 \"업주\"의 영업을 방해하는 내용을 기재하는 경우 \n9) 자신의 업소를 홍보할 목적으로 직접 또는 제3자(리뷰대행업체 등)을 통하여 위법 부당한 방법으로 허위 또는 과장된게시글을 게재하는 경우 \n10) 허위주문 또는 주문취소 등 위법 부당한 방법으로 리뷰권한을 획득하여 \"회원\" 또는 제3자의 계정을 이용하여 게시글을 게시하는 경우 \n11) 의미 없는 문자 및 부호에 해당하는 경우 \n12) 제3자 등으로부터 권리침해신고가 접수된 경우 \n13) 관계법령에 위반된다고 판단되는 경우 \n14) 기타 회사의 서비스 약관, 운영정책 등 위반행위를 할 우려가 있거나 위반행위를 한 경우 \n\n제 12 조 (회원게시물의 관리)\n\n1. \"회원\"의 \"게시물\"이 정보통신망법 및 저작권법 등 관련법에 위반되는 내용을 포함하는 경우, 권리자는 관련법이 정한 절차에 따라 해당 \"게시물\"의 게시중단 및 삭제 등을 요청할 수 있으며, 회사는 관련법에 따라 조치를 취하여야 합니다. \n2. 회사는 전항에 따른 권리자의 요청이 없는 경우라도 권리침해가 인정될 만한 사유가 있거나 기타 회사 정책 및 관련법에 위반되는 경우에는 관련법에 따라 해당 \"게시물\"에 대해 임시조치 등을 취할 수 있습니다. \n3. 본 조에 따른 세부절차는 정보통신망법 및 저작권법이 규정한 범위 내에서 회사가 정한 게시중단요청서비스에 따릅니다. \n- 게시중단요청: help@woowahan.com \n\n제 13 조 (이용제한 등)\n\n1. \"회사\"는 \"이용자\"가 이 약관의 의무를 위반하거나 \"서비스\"의 정상적인 운영을 방해한 경우, 주의, 경고, 일시정지, 영구이용정지, 계약해지 등의 (삭제)조치를 즉시 취할 수 있으며, \"이용자\"는 법적책임을 부담합니다. \n2. \"회사\"는 \"주민등록법\"을 위반한 명의도용 및 결제도용, 전화번호 도용, \"저작권법\"(삭제)을 위반한 불법프로그램의 제공 및 운영방해, \"정보통신망 이용촉진 및 정보보호 등에 관한 법률\"을 위반한 불법통신 및 해킹, 악성프로그램의 배포, 접속권한 초과행위, \"여신전문금융업법\"을 위반한 \"이용자\"의 신용카드 부정거래 등 이와 유사한 형태의 부정행위 등과 같이 관련법을 위반한 경우에는 주의, 경고, 일시정지, 영구이용정지, 계약해지 등의 조치를 즉시 취할 수 있으며, \"이용자\"는 법적책임을 부담할 수 있습니다. \n3. 회사는 회원이 계속해서 1년 이상 로그인하지 않는 경우, 회원정보의 보호 및 운영의 효율성을 위해 이용을 제한할 수 있습니다. \n4. 본 조의 이용제한 범위 내에서 제한의 조건 및 세부내용은 회사의 이용제한정책에서 정하는 바에 의합니다. \n5. 본 조에 따라 서비스 이용을 제한하거나 계약을 해지하는 경우에는 회사는 제15조[회원에 대한 통지]에 따라 통지합니다. \n6. \"회원\"은 본 조에 따른 이용제한 등에 대해 \"회사\"가 정한 절차에 따라 이의신청을 할 수 있습니다. 이 때 이의가 정당하다고 회사가 인정하는 경우 \"회사\"는 즉시 서비스의 이용을 재개합니다. \n7. 본 조에 따라 이용제한이 되는 경우 서비스 이용을 통해 획득한 혜택 등도 모두 이용중단, 또는 소멸되며, \"회사\"는 이에 대해 별도로 보상하지 않습니다. \n\n제 14 조 (권리의 귀속)\n\n1. \"서비스\"에 대한 저작권 및 지적재산권은 \"회사\"에 귀속됩니다. 단, \"회원\"의 \"게시물\" 및 제휴계약에 따라 제공된 저작물 등은 제외합니다. \n2. \"회사\"가 제공하는 \"서비스\"의 디자인, \"회사\"가 만든 텍스트, 스크립트(script), 그래픽, \"회원\" 상호간 전송 기능 등 \"회사\"가 제공하는 \"서비스\"에 관련된 모든 상표, 서비스 마크, 로고 등에 관한 저작권 기타 지적재산권은 대한민국 및 외국의 법령에 기하여 \"회사\"가 보유하고 있거나 \"회사\"에게 소유권 또는 사용권이 있습니다. \n3. \"회원\"은 이 이용약관으로 인하여 서비스를 소유하거나 \"서비스\"에 관한 저작권을 보유하게 되는 것이 아니라, \"회사로부터 서비스의 이용을 허락 받게 되는바, 정보취득 또는 개인용도로만 \"서비스\"를 이용할 수 있습니다. \n4. \"회원\"은 명시적으로 허락된 내용을 제외하고는 \"서비스\"를 통해 얻어지는 정보를 영리 목적으로 사용, 복사, 유통하는 것을 포함하여, \"회사\"가 만든 텍스트, 스크립트, 그래픽의 \"회원\" 상호간 전송기능 등을 복사하거나 유통할 수 없습니다. \n5. \"회사\"는 서비스와 관련하여 \"회원\"에게 \"회사\"가 정한 이용조건에 따라 계정, ID, 콘텐츠 등을 이용할 수 있는 이용권만을 부여하며, 이용자는 회사를 이용함으로써 얻은 정보를 회사의 사전 승낙 없이 복제, 송신, 출판, 배포, 방송 등 기타 방법에 의하여 영리 목적으로 이용하거나 제3자에게 이용, 양도, 판매, 담보목적으로 제공하여서는 안됩니다. \n\n제 15 조 (회원에 대한 통지)\n\n1. \"회사\"가 \"회원\"에 대한 통지를 하는 경우, \"회원\"이 가입신청 시 \"회사\"에 제출한 전자우편 주소나 휴대전화번호 등으로 할 수 있습니다. \n2. \"회사\"는 불특정다수 \"회원\"에 대한 통지의 경우, 1주일 이상 사이트에 게시함으로써 개별 통지에 갈음할 수 있습니다. \n\n제 16 조 (회사의 의무)\n\n1. \"회사\"는 관련법과 이 약관이 금지하거나 미풍양속에 반하는 행위를 하지 않으며, 계속적이고 안정적으로 \"서비스\"를 제공하기 위하여 최선을 다하여 노력합니다. \n2. \"회사\"는 \"회원\"이 안전하게 \"서비스\"를 이용할 수 있도록 개인정보(신용정보 포함)보호를 위해 개인정보처리방침을 수립하여 공시하고 준수합니다. \n3. 회사는 관계 법령이 정한 의무사항을 준수합니다.\n\n제 17 조 (개별 서비스에 대한 약관 및 이용조건)\n\n1. \"회사\"는 개별 서비스와 관련한 별도의 약관 및 이용정책을 둘 수 있으며, 개별서비스에서 별도로 적용되는 약관에 대한 동의는 \"회원\"이 개별서비스를 최초로 이용할 경우 별도의 동의절차를 거치게 됩니다. 이 경우 개별 서비스에 대한 이용약관 등이 본 약관에 우선합니다. \n2. 전항에도 불구하고 \"회사\"는 개별 서비스에 대한 이용정책에 대해서는 \"서비스\"를 통해 공지할 수 있으며, \"이용자\"는 이용정책을 숙지하고 준수하여야 합니다. \n\n제 18 조 (포인트)\n\n1. 포인트는 \"서비스\"를 통해 \"재화 등\"을 구매하는 경우 대금 결제 수단으로 사용할 수 있는 현금 등가 등의 결제수단을 의미합니다. \n2. 포인트는 \"회원\"의 구매활동, 이벤트 참여 등에 따라 \"회사\"가 적립, 부여하는 무료 포인트와 \"회원\"이 유료로 구매하는 유료포인트로 구분됩니다. \n3. 무료포인트의 유효기간은 적립일로부터 1년이며, 유료 포인트는 충전일로부터 5년이 경과하는 날까지 이용하지 않을 경우 상법상 소멸시효에 따라 소멸됩니다. 단, \"회사\"는 무료포인트의 유효기간을 변경할 수 있으며 이 경우 발급 시점에 \"회원\"에게 고지합니다. \n4. \"회사\"가 무상으로 적립 또는 부여하는 무료포인트는 현금환급 신청이 불가합니다. \n5. \"회사\"는 \"회원\"이 유료포인트에 대한 환급을 요구할 경우, 환급수수료를 공제하고 환급할 수 있으며, 환급조건 및 환급수수료에 대한 구체적인 내용은 서비스 페이지를 통해 안내합니다. \n6. \"회원\" 탈퇴 시 미 사용한 무료포인트는 소멸되며, \"회사\"는 소멸되는 무료 포인트에 대해서 별도의 보상을 하지 않습니다. \n7. \"회사\"는 \"회원\"이 포인트를 적립, 구매, 사용하는 경우 휴대폰인증, I-PIN 등 \"회사\"가 정한 인증절차를 거치도록 할 수 있습니다. \n8. \"회사\"는 포인트 적립기준, 사용조건 및 제한 등에 관한 사항을 서비스 화면에 별도로 게시하거나 통지합니다. \n\n제 19 조 (할인쿠폰)\n\n1. 할인쿠폰은 \"회사\"의 이벤트 프로모션 참여, \"업주\" 발급, \"회사\"의 정책에 따른 \"회원\" 등급별 부여 등을 통하여 \"회원\"에게 지급되며, \"할인쿠폰\"별 유효기간, 할인금액 및 사용방법 등은 개별 안내사항을 통하여 확인 가능합니다. \n2. 할인쿠폰은 현금으로 환급될 수 없으며, 할인쿠폰에 표시된 유효기간이 만료되거나 이용계약이 종료되면 소멸합니다. \n3. \"회사\"는 \"회원\"이 부정한 목적과 방법으로 할인쿠폰 등을 획득하거나 사용하는 사실이 확인될 경우, 해당 이용자에 대한 할인쿠폰을 회수 또는 소멸시키거나 회원자격을 제한할 수 있습니다. \n4. 할인쿠폰의 제공내용 및 운영방침은 \"회사\"의 정책에 따라 달라질 수 있습니다. \n\n제 20 조 (개인정보보호)\n\n1. \"회사\"는 \"회원\"의 개인정보를 보호하기 위하여 정보통신망법 및 개인정보 보호법 등 관계 법령에서 정하는 바를 준수합니다. \n2. \"회사\"는 \"회원\"의 개인정보를 보호하기 위한 개인정보처리방침을 수립하여 서비스 초기화면에 게시합니다. 다만, 개인정보처리방침의 구체적 내용은 연결화면을 통하여 볼 수 있습니다. \n3. \"회사\"는 관련법령 및 개인정보처리방침에 따라 \"회원\"의 개인정보를 최대한 보호하기 위하여 노력합니다. \n4. \"회사\"의 공식 사이트 이외의 링크된 사이트에서는 \"회사\"의 개인정보처리방침이 적용되지 않습니다. 링크된 사이트 및 구매 상품이나 서비스를 제공하는 제3자의 개인정보 취급과 관련하여는 해당 사이트 및 해당 제3자의 개인정보처리방침을 확인할 책임이 \"회원\"에게 있으며, \"회사\"는 이에 대하여 책임을 부담하지 않습니다. \n\n제 21 조 (주문 및 결제)\n\n1. \"재화 등\"에 대한 매매계약은 \"회원\"이 \"업주\"가 제시한 상품의 판매 조건에 응하여 청약의 의사표시를 하고 이에 대하여 \"업주\"가 승낙의 의사표시를 함으로써 \"회원\"과 \"업주\"간에 체결됩니다. \n2. \"회원\"은 다음 또는 이와 유사한 방법에 의한 구매를 신청할 수 있습니다. \n1) 전화주문서비스 \n2) 바로결제서비스(수취인 정보의 입력 및 결제수단의 선택 포함) \n3. \"회사\"가 \"업주\" 등 제3자에게 이용자의 개인정보를 제공할 필요가 있는 경우 ① 개인정보를 제공받는 자, ② 개인정보를 제공받는 자의 개인정보 이용목적, ③ 제공하는 개인정보의 항목, ④ 개인정보를 제공받는 자의 개인정보 보유 및 이용기간을 구매자에게 알리고 동의를 받습니다. \n4. \"회사\"가 제3자에게 구매자의 개인정보를 처리할 수 있도록 업무를 위탁하는 경우에는 ① 개인정보 처리위탁을 받는 자, ② 개인정보 처리위탁을 하는 업무의 내용을 구매자에게 알리고 동의를 받습니다. 다만, 「정보통신망 이용촉진 및 정보보호 등에 관한 법률」에서 정하고 있는 방법으로 개인정보 처리방침을 통해 알림으로써 동의절차를 갈음할 수 있습니다. \n5. \"회원\"이 전화주문서비스를 통해 업주에게 직접 주문을 경우, 회사는 주문내역 및 취소, 환불, 거래정보 등에 대해 관여하거나, 이에 대한 일체의 책임을 부담하지 않습니다. \n6. \"회사\"는 바로결제 방식에 의한 대금지급방법으로 신용카드 결제, 핸드폰 결제, 포인트, 기타 \"회사\"가 추가 지정하는 결제수단 등을 제공할 수 있습니다. \n7. \"회원\"이 바로결제서비스를 주문을 하는 경우 주소, 연락처 등 배송지 정보에 대한 정확한 정보를 기재해야 하며, \"회원\"이 입력한 정보 및 그 정보와 관련하여 발생한 책임과 불이익은 \"회원\"이 부담합니다. \n8. \"회사\"는 \"회원\"의 바로결제내역을 서비스 화면을 통해 확인할 수 있도록 조치하며, 전자상거래등에서의 소비자보호에 관한 법률에 규정에 의해 일정기간 동안 보존 할 수 있습니다. \n\n제 22 조 (배달)\n\n1. \"재화 등\"에 대한 배달은 \"업주\"의 책임에 따라 제공되며, \"회사\"는 주문정보의 중계 이 외에 \"재화 등\"의 배달(\"업주\"가 입력하는 배달 예상시간에 대한 정보 전송 포함)에 대한 책임을 부담하지 않습니다. 단, 제25조에 따른 배달대행서비스의 특칙이 적용되는 경우에는 그러하지 않습니다. \n2. \"업주\"는 \"재화 등\"에 대한 배달을 수행하기 위해 배달 이용요금을 \"이용자\"에게 부과할 수 있으며, 배달거리, 배달시간 등에 따라 배달 이용요금을 달리 정할 수 있습니다. 부과되는 배달 이용요금은 \"재화 등\"의 배달상품을 결제하기 전에 안내합니다. \n3. 배달과 관련하여 \"업주\"와 \"이용자\", 배달대행업체, 금융기관 등의 사이에 분쟁 등이 발생하면 관련 당사자가 해결해야 하며, \"회사\"는 이에 대해 어떠한 책임도 부담하지 않습니다. \n4. 배달 이용요금의 취소·환불은 제23조의 규정을 준용합니다. 단 \"회사\"가 인정하는 특별한 사정이 없는 한 \"재화 등\"에 대한 배달 상품과 분리하여 취소·환불되지 않습니다. \n\n제 23 조 (취소・환불)\n\n1. \"재화 등\"에 대한 취소 및 환불 규정은 전자상거래등에서의 소비자보호에 관한 법률 등 관련 법령을 준수합니다. 또한, 음식배달의 성격에 따라 음식 조리가 시작된 이후에는 \"업주\"에게 회복할 수 없는 손해가 발생하므로 단순변심에 의한 청약철회 등은 할 수 없습니다. \n2. \"회사\" 및 \"업주\"는 별도의 취소 및 환불 관련 규정을 정할 수 있으며, 이 경우 별도 계약 및 이용조건상의 취소 및 환불 규정이 우선 적용됩니다. \n3. \"회사\"는 \"회원\"이 본 조에 따라 바로결제를 통한 구매에 대해 청약철회를 하고 \"업주\"의 승인이 있는 경우 또는 구매 신청한 상품이 품절 등의 사유로 인도 또는 제공을 할 수 없음을 인지한 경우 지체 없이 그 사유를 \"회원\"에게 통지하고, 바로결제 내역을 취소하거나 대금을 받은 경우에는 환급에 필요한 조치를 취합니다. \n4. 취소와 환불은 \"업주\"와 확인 후 처리 하게 되며, 신용카드결제 및 핸드폰결제는 신용카드사 및 이동통신사의 환불기준에 의거하여 처리가 완료되는데 시일이 소요될 수 있습니다. \n\n제 24 조 (책임제한)\n\n1. \"회사\"는 \"업주\"와 \"회원\" 간의 상품거래를 중개하는 플랫폼 서비스만을 제공할 뿐, \"재화 등\"을 판매하는 당사자가 아니며, \"재화 등\"에 대한 정보 및 배송, 하자 등에 대한 책임은 \"업주\"에게 있습니다. \n2. \"회사\"는 \"업주\"가 게재한 정보, 자료, 사실의 신뢰도, 정확성 등 내용에 관해서는 책임을 지지 않습니다. \n3. \"회사\"는 천재지변 또는 이에 준하는 불가항력으로 인하여 \"서비스\"를 제공할 수 없는 경우에는 서비스 제공에 관한 책임이 면제됩니다. \n4. \"회사\"는 \"회원\"의 귀책사유로 인한 \"서비스\" 이용의 장애에 대하여는 책임을 지지 않습니다. \n5. \"회사\"는 \"회원\" 및 \"업주\"가 게재한 이용후기, 맛집 평가, 사진 등 정보/자료/사실의 신뢰도, 정확성에 대해서는 책임을 지지 않습니다. \n6. \"회사\"는 제3자가 서비스 내 화면 또는 링크된 웹사이트를 통하여 광고한 제품 또는 서비스의 내용과 품질에 대하여 감시할 의무 기타 어떠한 책임도 지지 아니합니다. \n7. \"회사\"는 \"회원\"이 서비스를 이용하여 기대하는 수익을 상실한 것에 대하여 책임을 지지 않으며, 그 밖의 서비스를 통하여 얻은 자료로 인한 손해에 관하여 책임을 지지 않습니다. \n8. \"회사\"는 \"회원\"간 또는 \"회원\"과 제3자 상호간에 서비스를 매개로 하여 거래 등을 한 경우에는 책임이 면제됩니다. \n9. \"회사\" 및 \"회사\"의 임직원 그리고 대리인은 고의 또는 중대한 과실이 없는 한 다음과 같은 사항으로부터 발생하는 손해에 대해 책임을 지지 아니합니다. \n1) 회원 정보의 허위 또는 부정확성에 기인하는 손해\n2) 서비스에 대한 접속 및 서비스의 이용과정에서 \"회원\"의 귀책사유로 발생하는 손해 \n3) 서버에 대한 제3자의 모든 불법적인 접속 또는 서버의 불법적인 이용으로부터 발생하는 손해 및 제3자의 불법적인 행위를 방지하거나 예방하는 과정에서 발생하는 손해 \n4) 제3자가 서비스를 이용하여 불법적으로 전송, 유포하거나 또는 전송, 유포되도록 한 모든 바이러스, 스파이웨어 및 기타 악성 프로그램으로 인한 손해 \n\n제 25 조 (배달대행서비스에 대한 특칙)\n\n1. \"배달대행서비스\"란 \"회사\"가 \"업주\"를 대신하여 상품을 \"업주\"로부터 \"회원\" 또는 \"비회원\"에게 배달하는 서비스를 말합니다. \n2. \"회사\"는 \"배달대행서비스\"의 특성을 고려하여, 서비스 이용시간 및 대상 상품 등을 별도로 정해서 운영할 수 있습니다. \n3. \"회사\"는 \"배달대행서비스\"에 대한 이용요금을 부과할 수 있으며, 배달상품, 배달거리, 배달시간 등에 따라 이용요금을 달리 정할 수 있습니다. 단, 부과되는 이용요금은 사전에 안내합니다. \n4. \"회원\"은 \"배달대행서비스\"가 적용되는 \"재화 등\"에 대해 상품과 배달대행서비스를 구분해서, 결제, 청약철회, 환불 등을 요청할 수 없습니다. 단, \"회사\"가 인정하는 경우에는 구분해서 환불 처리 등을 할 수 있습니다. \n5. \"회사\"는 \"배달대행서비스의 제공과 관련해서 \"업주\"로부터 상품을 전달받은 이후 \"회사\"의 귀책사유로 발생한 배달사고, 음식물의 훼손 등 이에 상응하는 책임을 부담하며, 만약 원만한 분쟁해결이 이루어지지 않는 경우, 공정거래위원회의 소비자분쟁해결기준에 따라 해결합니다. \n6. \"회사\"는 \"회원\"과 수 차례 연락을 시도한 후 연락이 되지 않는 경우 배달음식이 변질되거나 부패될 우려로 식품위생법상 위반될 여지가 있어 별도로 보관하지 않으며, 재배달 또는 환불처리가 어려울 수 있습니다. \n7. \"회원\"은 다음 각호의 귀책사유로 조리된 음식을 수령하지 못하더라도 \"회원\"은 대금지급의무 또는 손해배상금을 부담합니다. \n1) 배달주소지를 \"회원\"의 과실로 잘못 등록하거나 작성한 경우 단, 배달주소지 오류는 재배달 하지 않습니다. \n2) \"회원\"과 수차례 연락을 시도하였으나 연락이 되지 않는 경우 단, 회사정책에 따라 서비스 이용(ID 정지 등)에 제한이 있을 수 있습니다. \n8. 만나서 결제 방식을 통해 \"회원\"이 주문할 경우 \"업주\" 또는 배달원에게 대금지급을 하지 않을 경우 서비스 이용(ID 정지 등)에 제한이 있을 수 있습니다. 또한 경범죄처벌법 등 관련 법령에 따라 책임을 부담합니다. \n9. 티켓룸 앱 내에 등록된 메뉴 외 요청사항에서 \"회원\"이 작성한 추가메뉴는 배달상품을 수령하는 시점에 추가메뉴에 대한 상품을 배달하지 않으며, 해당 상품의 현장결제를 금지합니다. \n10. 티켓룸 앱 내에 등록하여 고객님이 주문한 조리된 음식 등 상품 이외에 다음의 각호와 같이 회사에서 허용(등록)하지 않는 상품 또는 용역행위를 요청한 경우 이행할 수 없는 주문상품으로 판단하여 강제 주문 취소가 진행 될 수 있습니다. \n1) 담배 구매요청 \n2) 티켓룸 앱 내에 등록되지 않은 주류 구매 요청 \n3) 음란물 상품 등 구매요청 \n4) 기타 용역행위의 요청 \n11. 본 조는 \"회사\"의 배달대행 용역에 대한 책임범위 등을 규정하며, 이 약관의 다른 규정에 따라 \"업주\"가 취급하는 \"재화 등\"에 대한 책임소재에 대해서는 책임을 부담하지 않습니다. \n\n제 26 조 (\"재화 등\"에 대한 판매 서비스)\n\n1. \"판매서비스\"란 \"회사\"가 \"이용자\"에게 \"재화 등\"을 직접 판매하는 서비스를 말합니다. \n2. \"회사\"는 이용자와 재화 등의 공급시기에 관하여 별도의 약정이 없는 이상, 이용자가 청약을 한 날부터 7일 이내에 재화 등을 배송할 수 있도록 주문제작, 포장 등 기타의 필요한 조치를 취합니다. 다만, \"회사\"가 이미 재화 등의 대금의 전부 또는 일부를 받은 경우에는 대금의 전부 또는 일부를 받은 날부터 3영업일 이내에 조치를 취합니다. 이때 \"회사\"는 이용자가 재화 등의 공급 절차 및 진행 사항을 확인할 수 있도록 적절한 조치를 합니다. \n3. \"회사\"는 이용자가 구매한 \"재화 등\"에 대해 배송수단, 수단별 배송비용 부담자, 수단별 배송기간 등을 명시합니다. 만약 \"회사\"가 약정 배송기간을 초과한 경우에는 그로 인한 이용자의 손해를 배상하여야 합니다. 다만 \"회사\"가 고의․과실이 없음을 입증한 경우에는 그러하지 아니합니다. \n4. \"회사\"는 이용자가 구매신청한 재화 등이 품절 등의 사유로 인도 또는 제공을 할 수 없을 때에는 지체 없이 그 사유를 이용자에게 통지하고 사전에 재화 등의 대금을 받은 경우에는 대금을 받은 날부터 3영업일 이내에 환급하거나 환급에 필요한 조치를 취합니다. \n5. \"회사\"와 \"재화 등\"의 구매에 관한 계약을 체결한 이용자는 「전자상거래 등에서의 소비자보호에 관한 법률」 제13조 제2항에 따른 계약내용에 관한 서면을 받은 날(그 서면을 받은 때보다 재화 등의 공급이 늦게 이루어진 경우에는 재화 등을 공급받거나 재화 등의 공급이 시작된 날을 말합니다)부터 7일 이내에는 청약의 철회를 할 수 있습니다. 다만, 청약철회에 관하여 「전자상거래 등에서의 소비자보호에 관한 법률」에 달리 정함이 있는 경우에는 동 법 규정에 따릅니다. \n6. 이용자는 재화 등을 배송 받은 경우 다음 각 호의 1에 해당하는 경우에는 반품 및 교환을 할 수 없습니다. \n1) 이용자에게 책임 있는 사유로 재화 등이 멸실 또는 훼손된 경우(다만, 재화 등의 내용을 확인하기 위하여 포장 등을 훼손한 경우에는 청약철회를 할 수 있습니다) \n2) 이용자의 사용 또는 일부 소비에 의하여 재화 등의 가치가 현저히 감소한 경우 \n3) 시간의 경과에 의하여 재판매가 곤란할 정도로 재화등의 가치가 현저히 감소한 경우 \n4) 같은 성능을 지닌 재화 등으로 복제가 가능한 경우 그 원본인 재화 등의 포장을 훼손한 경우 \n7. 제한되는 사실을 소비자가 쉽게 알 수 있는 곳에 명기하거나 시용상품을 제공하는 등의 조치를 하지 않았다면 이용자의 청약철회 등이 제한되지 않습니다. \n8. 이용자는 제1항 및 제2항의 규정에 불구하고 재화 등의 내용이 표시·광고 내용과 다르거나 계약내용과 다르게 이행된 때에는 당해 재화 등을 공급받은 날부터 3월 이내, 그 사실을 안 날 또는 알 수 있었던 날부터 30일 이내에 청약철회 등을 할 수 있습니다. \n9. \"회사\"는 이용자로부터 재화 등을 반환 받은 경우 3영업일 이내에 이미 지급받은 재화 등의 대금을 환급합니다. \n10. \"회사\"는 위 대금을 환급함에 있어서 이용자가 신용카드 또는 전자화폐 등의 결제수단으로 재화 등의 대금을 지급한 때에는 지체 없이 당해 결제수단을 제공한 사업자로 하여금 재화 등의 대금의 청구를 정지 또는 취소하도록 요청합니다. \n11. 청약철회 등의 경우 공급받은 재화 등의 반환에 필요한 비용은 이용자가 부담합니다. \"회사\"는 이용자에게 청약철회 등을 이유로 위약금 또는 손해배상을 청구하지 않습니다. 다만 재화 등의 내용이 표시·광고 내용과 다르거나 계약내용과 다르게 이행되어 청약철회 등을 하는 경우 재화 등의 반환에 필요한 비용은 \"회사\"가 부담합니다. \n\n제 27 조 (분쟁해결)\n\n1. \"회사\"는 이용자가 제기하는 정당한 의견이나 불만을 반영하고 그 피해를 보상처리하기 위하여 고객상담 및 피해보상처리기구를 설치・운영합니다. \n2. \"회사\"는 이용자로부터 제출되는 불만사항 및 의견은 우선적으로 그 사항을 처리합니다. 다만, 신속한 처리가 곤란한 경우에는 이용자에게 그 사유와 처리일정을 즉시 통보해 드립니다. \n3. \"회사\"와 이용자 간에 발생한 전자상거래 분쟁과 관련하여 이용자의 피해구제신청이 있는 경우에는 공정거래위원회 또는 시·도지사가 의뢰하는 분쟁조정기관의 조정에 따를 수 있습니다. \n\n제 28 조 (준거법 및 관할법원)\n\n1. 이 약관의 해석 및 회사와 회원간의 분쟁에 대하여는 대한민국의 법을 적용합니다. \n2. 서비스 이용 중 발생한 회원과 회사간의 소송은 민사소송법에 의한 관할법원에 제소합니다. \n\n<부칙>\n\n1. 이 약관은 2018년 12월 18일부터 시행됩니다.\n";
                    Navigation.PushAsync(new TermsContentPage("상품권 거래 이용 약관", content));
                    PopupNavigation.Instance.RemovePageAsync(this);
                })
            });

            TermsListGrid.Children[1].GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    string content = "제 1 장 총칙\n\n\n제 1 조 (목적)\n\n본 약관은 ㈜비긴(이하 \"회사\"라 합니다)이 운영하는 인터넷사이트를 통하여 제공하는 전자지급결제대행서비스, 결제대금예치서비스 및 선불전자지급수단의 발행 및 관리 서비스(이하 통칭하여 \"서비스\"라 합니다)를 \"이용자\"가 이용함에 있어, \"회사\"와 \"이용자\"간 권리, 의무 및 \"이용자\"의 서비스 이용절차 등에 관한 사항을 규정하는 것을 그 목적으로 합니다. \n\n제 2 조 (용어의 정의)\n\n① 본 약관에서 정하는 용어의 정의는 다음과 같습니다. \n1. \"전자금융거래\"라 함은 \"회사\"가 전자적 장치를 통하여 전자금융업무를 제공하고, \"이용자\"가 \"회사\"의 종사자와 직접 대면하거나 의사소통을 하지 아니하고 자동화된 방식으로 이를 이용하는 거래를 말합니다. \n2. \"전자지급거래\"라 함은 자금을 주는 자(이하 ‘지급인’이라 합니다)가 \"회사\"로 하여금 전자지급수단을 이용하여 자금을 받는 자(이하 ‘수취인’이라 합니다)에게 자금을 이동하게 하는 전자금융거래를 말합니다. \n3. \"이용자\"라 함은 전자금융거래를 위하여 본 약관에 따라 \"회사\"가 제공하는 전자금융거래 서비스를 이용하는 회원을 말합니다. \n4. \"전자적 장치\"라 함은 전자금융거래정보를 전자적 방법으로 전송하거나 처리하는데 이용되는 장치로서 현금자동지급기, 자동입출금기, 지급용단말기, 컴퓨터, 전화기 그 밖에 전자적 방법으로 정보를 전송하거나 처리하는 장치를 말합니다. \n5. \"접근매체\"라 함은 전자금융거래에 있어서 거래지시를 하거나 \"이용자\" 및 거래내용의 진실성과 정확성을 확보하기 위하여 사용되는 수단 또는 정보로서 서비스를 이용하기 위하여 \"회사\"에 등록된 아이디 및 비밀번호, 기타 \"회사\"가 지정한 수단을 말합니다. \n6. \"아이디\"란 \"이용자\"의 동일성 식별과 서비스 이용을 위하여 \"이용자\"가 설정하고 \"회사\"가 승인한 숫자와 문자의 조합을 말합니다. \n7. \"비밀번호\"란 \"이용자\"의 동일성 식별과 회원정보의 보호를 위하여 \"이용자\"가 설정하고 \"회사\"가 승인한 숫자와 문자 등의 조합을 말합니다. \n8. \"거래지시\"라 함은 \"이용자\"가 본 약관에 따라 \"회사\"에게 전자금융거래의 처리를 지시하는 것을 말합니다. \n9. \"오류\"라 함은 \"이용자\"의 고의 또는 과실 없이 전자금융거래가 본 약관 또는 \"이용자\"의 거래지시에 따라 이행되지 아니한 경우를 말합니다. \n10. \"전자적 침해행위\"란 해킹, 컴퓨터 바이러스, 논리폭탄, 메일폭탄, 서비스 거부 또는 고출력 전자기파 등의 방법으로 전자금융기반시설을 공격하는 행위를 말합니다. \n② 본 조 및 본 약관의 다른 조항에서 정의한 것을 제외하고는 전자금융거래법 등 관련법령에 정한 바에 의합니다. \n\n제 3 조 (약관의 명시 및 변경)\n\n① \"회사\"는 \"이용자\"가 전자금융거래를 하기 전에 본 약관을 서비스 페이지에 게시하고 \"이용자\"가 본 약관의 중요한 내용을 확인할 수 있도록 합니다. \n② \"회사\"는 \"이용자\"의 요청이 있는 경우 전자문서의 전송(전자우편을 이용한 전송을 포함합니다), 모사전송, 우편 또는 직접 교부 중 어느 하나 이상의 방법으로 본 약관의 사본을 \"이용자\"에게 교부합니다. \n③ \"회사\"가 본 약관을 변경하는 때에는 그 시행일 1월 전에 변경되는 약관을 금융거래정보 입력화면 또는 \"서비스\" 홈페이지에 게시함으로써 \"이용자\"에게 공지합니다. \n④ \"회사\"는 전항의 공지를 할 경우 \"이용자가 변경 내용에 동의하지 아니한 경우 공지한 날로부터 1개월 (공지기간)이내에 계약을 해지할 수 있으며, 계약해지의 의사표시를 하지 아니한 경우에는 변경에 동의한 것으로 본다\"라는 취지의 내용을 고지하거나 통지합니다. \n\n제 4 조 (거래내용의 확인)\n\n① \"회사\"는 \"서비스\" 페이지의 ‘마이페이지’, ‘바로결제내역’ 조회 화면을 통하여 \"이용자\"의 거래내용(\"이용자\"의 ‘오류정정 요구사실 및 처리결과에 관한 사항’을 포함합니다)을 확인할 수 있도록 하며, \"이용자\"의 요청이 있는 경우에는 요청을 받은 날로부터 2주 이내에 전자우편, 모사전송, 우편 또는 직접 교부의 방법으로 거래내용에 관한 서면을 교부합니다. \n② \"회사\"는 제1항에 따른 회원의 거래내용 서면교부 요청을 받은 경우 전자적 장치의 운영장애, 그 밖의 이유로 거래내용을 제공할 수 없는 때에는 즉시 회원에게 전자문서 전송(전자우편을 이용한 전송을 포함합니다)의 방법으로 그러한 사유를 알려야 하며, 전자적 장치의 운영장애 등의 사유로 거래내용을 제공할 수 없는 기간은 제1항의 거래내용에 관한 서면의 교부기간에 산입하지 아니합니다. \n③ \"이용자\"가 제1항에서 정한 서면교부를 요청하고자 할 경우 다음의 주소 및 전화번호로 요청할 수 있습니다. \n1.주소: 서울특별시 송파구 위례성대로2(방이동, 장은빌딩) \n2.전자우편: help@woowahan.com \n3. 전화번호: (티켓룸) 1600-0987, (배민상회) 1600-4949 \n\n제 5 조 (거래지시의 철회 등)\n\n\"이용자\"가 \"회사\"의 전자금융거래서비스를 이용하여 전자지급거래를 한 경우, \"이용자\"는 지급의 효력이 발생하기 전까지 본 약관에서 정한 바에 따라 제4조 제3항에 기재된 연락처로 전자문서의 전송(전자우편을 이용한 전송을 포함합니다) 또는 서비스 페이지 내 철회에 의한 방법으로 거래지시를 철회할 수 있습니다. 각 서비스별 거래지시 철회의 효력 발생시기는 본 약관 제15조, 제20조 및 제25조에서 정하는 바와 같습니다. \n\n제 6 조 (오류의 정정 등)\n\n① \"이용자\"는 전자금융거래서비스를 이용함에 있어 오류가 있음을 안 때에는 \"회사\"에 대하여 그 정정을 요구할 수 있으며, \"회사\" 또는 가맹점은 회원의 정정 신청이 있는 경우 이를 조사하여 오류가 있는 경우 정정절차를 진행하고 정정요구를 받은 날부터 2주 이내에 그 결과를 \"이용자\"에게 알려드립니다. \n② \"회사\"는 스스로 전자금융거래에 오류가 있음을 안 때에도 이를 즉시 조사하여 처리한 후 그 결과를 2주 이내에 문서로 \"이용자\"에게 알려 드립니다. 다만, \"이용자\"의 주소가 분명하지 아니하거나 \"이용자\"가 요청한 경우에는 전화 또는 전자우편 등의 방법으로 알릴 수 있습니다. \n\n제 7 조 (전자금융거래 기록의 생성 및 보존)\n\n① \"회사\"는 \"이용자\"가 이용한 전자금융거래의 내용을 추적, 검색하거나 그 내용에 오류가 발생한 경우에 이를 확인하거나 정정할 수 있는 기록을 생성하여 보존합니다. \n② 제1항의 규정에 따라 \"회사\"가 보존하여야 하는 기록의 종류 및 보존방법은 아래 각 호와 같습니다. \n1. 제1항의 대상이 되는 거래내용 중 대상기간이 5년인 것은 다음 각호와 같습니다. \n가. 전자금융거래의 종류(보험계약의 경우에는 보험계약의 종류를 말한다) 및 금액, 전자금융거래의 상대방에 관한 정보 \n나. 전자금융거래의 거래일시, 전자적 장치의 종류 및 전자적 장치를 식별할 수 있는 정보 \n다. 전자금융거래가 계좌를 통하여 이루어지는 경우 거래계좌의 명칭 또는 번호(보험계약의 경우에는 보험증권번호를 말한다) \n라. \"회사\"가 전자금융거래의 대가로 받은 수수료 \n마. 회원의 출금 동의에 관한 사항 \n바. 해당 전자금융거래와 관련한 전자적 장치의 접속기록 \n사. 전자금융거래의 신청 및 조건의 변경에 관한 사항 \n아. 건당 거래금액이 1만원을 초과하는 전자금융거래에 관한 기록 \n2. 제1항의 대상이 되는 거래내용 중 대상기간이 1년인 것은 다음 각호와 같습니다. \n가. 건당 거래금액이 1만원 이하인 전자금융거래에 관한 기록 \n나. 전자지급수단 이용과 관련된 거래승인에 관한 기록 \n다. \"이용자\"의 오류정정 요구사실 및 처리결과에 관한 사항 \n\n제 8 조 (전자금융거래정보의 제공금지)\n\n① \"회사\"는 전자금융거래서비스를 제공함에 있어서 취득한 \"이용자\"의 인적사항, \"이용자\"의 계좌, 접근매체 및 전자금융거래의 내용과 실적에 관한 정보 또는 자료를 법령에 의하거나 \"이용자\"의 동의를 얻지 아니하고 제3자에게 제공, 누설하거나 업무상 목적 외에 사용하지 아니합니다. \n② \"회사\"는 \"이용자\"가 안전하게 전자금융거래서비스를 이용할 수 있도록 \"이용자\"의 개인정보보호를 위하여 개인정보보호정책을 실시하며, 이에 따라 \"이용자\"의 개인정보보호를 하여야 할 의무가 있습니다. \"회사\"의 개인정보보호정책은 \"회사\" 홈페이지 또는 서비스 페이지에 링크된 개인정보취급방침 화면을 통하여 확인할 수 있습니다. \n\n제 9 조 (\"회사\"의 책임)\n\n① \"회사\"는 접근매체의 위조나 변조로 발생한 사고, 계약체결 또는 거래지시의 전자적 전송이나 처리과정에서 발생한 사고(전자금융거래를 위한 전자적 장치 또는 ‘정보통신망 이용촉진 및 정보보호 등에 관한 법률’ 제2조제1항제1호에 따른 정보통신망에 침입하여 거짓이나 그 밖의 부정한 방법으로 획득한 접근매체의 이용으로 발생한 사고로 인하여 이용자에게 그 손해가 발생한 경우)로 인하여 \"이용자\"에게 손해가 발생한 경우에는 그 손해를 배상할 책임을 부담합니다. \n② \"회사\"는 제1항에도 불구하고 다음 각호의 경우에는 그 책임의 전부 또는 일부를 \"이용자\"가 부담하게 할 수 있습니다. \n1. \"이용자\"가 접근매체를 제3자에게 대여하거나 사용을 위임하거나 양도 또는 담보 목적으로 제공하는 경우 \n2. 제3자가 권한 없이 \"이용자\"의 접근매체를 이용하여 전자금융거래를 할 수 있음을 알았거나 알 수 있었음에도 불구하고 \"이용자\"가 자신의 접근매체를 누설 또는 노출하거나 방치한 경우 \n3. \"회사\"가 보안강화를 위하여 전자금융거래 시 요구하는 추가적인 보안조치를 \"이용자\"가 정당한 사유 없이 거부하여 사고가 발생한 경우 \n4. 법인(‘중소기업기본법’ 제2조 제2항에 의한 소기업을 제외한다)인 \"이용자\"에게 손해가 발생한 경우로서 \"회사\"가 사고를 방지하기 위하여 보안절차를 수립하고 이를 철저히 준수하는 등 합리적으로 요구되는 충분한 주의의무를 다한 경우 \n③ \"회사\"는 \"이용자\"로부터의 거래지시가 있음에도 불구하고 천재지변 등 기타의 불가항력적인 사유로 처리 불가능하거나 지연된 경우로서 \"이용자\"에게 처리 불가능 또는 지연 사유를 통지한 경우(금융기관 또는 결제수단 발행업체나 통신판매업자가 통지한 경우를 포함합니다)에는 \"이용자\"에 대하여 이로 인한 책임을 지지 아니합니다. \n④ \"회사\"는 컴퓨터 등 정보통신설비의 보수점검, 교체의 사유가 발생하여 전자금융서비스의 제공을 일시적으로 중단할 경우에는 \"회사\"의 홈페이지를 통하여 \"이용자\"에게 중단 일정 및 중단 사유를 사전에 공지합니다. \n\n제 10 조 (\"회사\"의 안정성 확보 의무)\n\n\"회사\"는 전자금융거래가 안전하게 처리될 수 있도록 선량한 관리자로서의 주의를 다하며, 전자금융거래의 안전성과 신뢰성을 확보할 수 있도록 전자금융거래의 종류별로 전자적 전송이나 처리를 위한 인력, 시설, 전자적 장치 등의 정보기술부문 및 전자금융업무에 관하여 금융위원회가 정하는 기준을 준수합니다. \n\n제 11 조 (분쟁처리 및 분쟁조정)\n\n① \"이용자\"는 \"회사\"의 홈페이지 메인 화면 하단에 게시된 분쟁처리 책임자 및 담당자에 대하여 전자금융거래와 관련한 의견 및 불만의 제기, 손해배상의 청구 등의 분쟁처리를 요구할 수 있습니다. \n② \"이용자\"가 \"회사\"에 대하여 분쟁처리를 신청한 경우에는 \"회사\"는 15일 이내에 이에 대한 조사 또는 처리 결과를 \"이용자\"에게 안내합니다. \n③ \"이용자\"는 \"회사\"의 분쟁처리결과에 대하여 이의가 있을 경우 ‘금융위원회의 설치 등에 관한 법률’ 제51조의 규정에 따른 금융감독원의 금융분쟁조정위원회나 ‘소비자기본법’ 제60조 제1항의 규정에 따른 한국소비자원의 소비자분쟁조정위원회에 \"회사\"의 전자금융거래서비스의 이용과 관련한 분쟁조정을 신청할 수 있습니다. \n\n제 12 조 (약관외 준칙)\n\n본 약관에서 정하지 아니한 사항(용어의 정의 포함)에 대하여는 전자금융거래법, 전자상거래 등에서의 소비자 보호에 관한 법률, 여신전문금융업법 등 소비자보호 관련 법령 및 개별약관에서 정한 바에 따릅니다. \n\n제 13 조 (관할)\n\n\"회사\"와 \"이용자\"간에 발생한 분쟁에 관한 관할은 민사소송법에서 정한 바에 따릅니다. \n\n제 2 장 전자지급결제대행 서비스\n\n\n제 14 조 (정의)\n\n본 장에서 정하는 용어의 정의는 다음과 같습니다. \n1. \"전자지급결제대행 서비스\"라 함은 전자적 방법으로 재화 또는 용역(이하 ‘재화 등’이라고 합니다)의 구매에 있어서 지급결제정보를 송신하거나 수신하는 것 또는 그 대가의 정산을 대행하거나 매개하는 서비스를 말합니다. \n2. \"이용자\"라 함은 본 약관에 동의하고 \"회사\"가 제공하는 전자지급결제대행 서비스를 이용하는 자를 말합니다. \n\n제 15 조 (거래지시의 철회)\n\n① \"이용자\"가 전자지급결제대행서비스를 이용한 경우, \"이용자\"는 거래 지시된 금액의 정보에 대하여 수취인의 계좌가 개설되어 있는 금융기관 또는 \"회사\"의 계좌의 원장에의 입금기록 또는 전자적 장치에의 입력이 끝나기 전까지 거래지시를 철회할 수 있습니다. \n② \"회사\"는 \"이용자\"의 거래지시의 철회에 따라 지급거래가 이루어지지 않은 경우 수령한 자금을 \"이용자\"에게 반환하여야 합니다. \n\n제 16 조 (접근매체의 관리)\n\n① \"회사\"는 전자지급결제대행 서비스 제공 시 접근매체를 선정하여 \"이용자\"의 신원, 권한 및 거래지시의 내용 등을 확인합니다. \n② \"회사\"는 회원이 접근매체의 분실 또는 도난 등을 통지하지 않아 발생하는 손해에 대하여 책임지지 않습니다. \n③ \"이용자\"는 접근매체를 사용함에 있어서 다른 법률에 특별한 규정이 없는 한 다음 각 호의 행위를 하여서는 아니 됩니다. \n1. 접근매체를 양도하거나 양수하는 행위 \n2. 대가를 수수・요구 또는 약속하면서 접근매체를 대여받거나 대여하는 행위 또는 보관・전달・유통하는 행위 \n3. 범죄에 이용할 목적으로 또는 범죄에 이용될 것을 알면서 접근매체를 대여받거나 대여하는 행위 또는 보관・전달・유통하는 행위 \n4. 접근매체를 질권의 목적으로 하는 행위 \n5. 제1호부터 제4호까지의 행위를 알선하거나 광고하는 행위 \n④ \"이용자\"는 자신의 접근매체를 제3자에게 누설 또는 노출하거나 방치하여서는 안되며, 접근매체의 도용이나 위조 또는 변조를 방지하기 위하여 충분한 주의를 기울여야 합니다. \n⑤ \"회사\"는 \"이용자\"로부터 접근매체의 분실이나 도난 등의 통지를 받은 때에는 제3자가 그 접근매체를 사용함으로 인하여 \"회원\"에게 손해가 발생하지 않도록 신속히 필요한 조치를 수행합니다. \n\n제 3 장 결제대금예치서비스\n\n\n제 17 조 (정의)\n\n본 장에서 사용하는 용어의 정의는 다음과 같습니다. \n1. ‘결제대금예치서비스’라 함은 서비스 페이지에서 이루어지는 선불식 통신판매에 있어서, \"회사\"가 소비자가 지급하는 결제대금을 예치하고 배송이 완료되는 등 구매가 확정된 후 재화 등의 대금을 판매자에게 지급하는 서비스를 말합니다. \n2. ‘선불식 통신판매’라 함은 소비자가 재화 등을 공급받기 전에 미리 대금의 전부 또는 일부를 지급하는 방식의 통신판매를 말합니다. \n3. ‘판매자’라 함은 본 약관에 동의하고 \"회사\"가 운영하는 서비스에 입점하여 통신판매를 하는 자를 말합니다. \n4. ‘소비자’라 함은 본 약관에 동의하고 \"회사\"의 서비스에 입점한 판매자로부터 재화 등을 구매하는 자로서 전자상거래 등에서의 소비자보호에 관한 법률 제2조 제5호의 요건에 해당하는 자를 말합니다. \n5. ‘\"이용자\"’라 함은 본 조 제3호 및 제4호의 ‘판매자’와 ‘소비자’를 말합니다. \n\n제 18 조 (적용범위)\n\n본 약관은 \"이용자\"가 서비스를 통한 통신판매를 이용하는 경우에 적용됩니다. \n\n제 19 조 (예치된 결제대금의 지급방법)\n\n① 소비자(그 소비자의 동의를 얻은 경우에는 재화 등을 공급받을 자를 포함한다. 이하 제2항 및 제3항에서 같습니다)는 재화 등을 공급받은 사실을 재화 등을 공급받은 날부터 3영업일 이내에 \"회사\"에 통보하여야 합니다. \n② \"회사\"는 소비자로부터 재화 등을 공급받은 사실을 통보 받을 경우 \"회사\"가 정한 기일 내에 판매자에게 결제대금을 지급합니다. \n③ \"회사\"는 소비자가 재화 등을 공급받은 날부터 3영업일이 지나도록 정당한 사유의 제시 없이 그 공급받은 사실을 \"회사\"에 통보하지 아니하거나, 재화 등의 특성 상 철약철회가 제한되는 것으로 사전에 소비자에게 고지한 경우에 소비자의 동의 없이 판매자에게 결제대금을 지급할 수 있습니다. \n④ \"회사\"가 판매자에게 결제대금을 지급하기 전에 소비자가 그 결제대금을 환급 받을 정당한 사유가 발생한 경우에는 그 결제대금을 소비자에게 환급합니다. \n\n제 20 조 (거래지시의 철회)\n\n① \"이용자\"가 결제대금예치서비스를 이용한 경우, \"이용자\"는 거래 지시된 금액의 정보가 수취인이 지정한 전자적 장치에 도달한 때까지 거래지시를 철회할 수 있습니다. \n② \"회사\"는 \"이용자\"의 거래지시의 철회에 따라 지급거래가 이루어지지 않은 경우 수령한 자금을 \"이용자\"에게 반환하여야 합니다. \n\n제 21 조 (준용규정)\n\n제2장 결제대행서비스 제16조는 본 장 결제대금예치서비스에 준용합니다. \n\n제 4 장 선불전자지급수단의 발행 및 관리 서비스\n\n\n제 22 조 (정의)\n\n본 장에서 사용하는 용어의 정의는 다음과 같습니다. \n1. ‘선불전자지급수단’이라 함은 \"회사\"의 서비스 충전금 등 \"회사\"가 발행 당시 미리 \"이용자\"에게 공지한 전자금융거래법상 선불전자지급수단을 말합니다. \n2. ‘\"이용자\"’라 함은 본 약관에 동의하고 판매자로부터 재화 등을 구매하고 선불전자지급수단을 결제수단으로 하여 그 대가를 지급하는 자를 말합니다. \n3. ‘판매자’라 함은 선불전자지급수단에 의한 거래에 있어서 \"이용자\"에게 재화 등을 판매하는 자를 말합니다. \n\n제 23 조 (접근매체의 관리)\n\n① \"회사\"는 \"이용자\"로부터 접근매체의 분실 또는 도난 등의 통지를 받기 전에 발생하는 선불전자지급수단 등에 저장된 금액에 대한 손해에 대하여는 책임지지 않습니다. \n② 제2장 결제대행서비스 제16조 제1항 내지 제4항은 본 장에 준용합니다. \n\n제 24 조 (환급 등)\n\n① \"이용자\"는 보유 중인 선불전자지급수단의 환급을 \"회사\"에 요구할 경우 미상환 잔액에 대해 환급 받을 수 있습니다. 다만, 상품 구매나 이벤트 등을 통하여 \"회사\"로부터 무상 제공받은 선불전자지급수단의 경우 환급이 불가능합니다. \n② \"이용자\"는 \"회사\"에서 정한 기한 및 이용방법에 따라 선불전자지급수단을 이용할 수 있으며 \"회사\"는 그 구체적인 사항을 서비스 페이지를 통해 공지합니다. \n\n제 25 조 (거래지시의 철회)\n\n\"이용자\"가 선불전자지급수단을 이용하여 자금을 지급하는 경우, \"이용자\"는 거래 지시 된 금액의 정보가 수취인이 지정한 전자적 장치에 도달한 때까지 거래지시를 철회할 수 있습니다. \n\n제 26 조 (유효기간)\n\n① \"회사\"는 \"이용자\"에 대하여 선불전자지급수단에 대한 유효기간을 설정할 수 있으며, \"이용자\"는 \"회사\"가 정한 유효기간 내에서만 선불전자지급수단을 사용할 수 있습니다. \n② \"회사\"는 서비스 페이지 등을 통하여 전항의 유효기간 설정 여부 및 그 기간을 공지합니다. \n\n<부칙>\n\n제1조 (적용일자) \n본 약관은 2019년 01월 11일부터 시행됩니다. \n";

                    Navigation.PushAsync(new TermsContentPage("전자금융 거래 이용약관 동의", content));
                    PopupNavigation.Instance.RemovePageAsync(this);
                })
            });

            TermsListGrid.Children[2].GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() =>
                {
                    PopupNavigation.Instance.RemovePageAsync(this);
                })
            });
        }

        private async void ConfirmBtn_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.RemovePageAsync(this);
        }
    }
}