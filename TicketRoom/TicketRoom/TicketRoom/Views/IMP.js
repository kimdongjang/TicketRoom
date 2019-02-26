<script type="text/javascript" src="https://code.jquery.com/jquery-1.12.4.min.js"></script>
<script type="text/javascript" src="https://cdn.iamport.kr/js/iamport.payment-1.1.5.js"></script>

var IMP = window.IMP; // ��������
IMP.init('imp36373625'); // 'iamport' ��� �ο����� "������ �ĺ��ڵ�"�� ���
    
    /* �߷� */
    
    //onclick, onload �� ���ϴ� �̺�Ʈ�� ȣ���մϴ�
IMP.request_pay({
    pg: 'inicis', // version 1.1.0���� ����.
    pay_method: 'card',
    merchant_uid: 'merchant_' + new Date().getTime(),
    name: '�ֹ���:�����׽�Ʈ',
    amount: 14000,
    buyer_email: 'iamport@siot.do',
    buyer_name: '�������̸�',
    buyer_tel: '010-1234-5678',
    buyer_addr: '����Ư���� ������ �Ｚ��',
    buyer_postcode: '123-456',
    m_redirect_url: 'https://www.yourdomain.com/payments/complete',
    app_scheme: 'iamportapp'
}, function (rsp) {// callback
    if (rsp.success) {// ���� ���� ��: ���� ���� �Ǵ� ������� �߱޿� ������ ���
        var msg = '������ �Ϸ�Ǿ����ϴ�.';
        msg += '����ID : ' + rsp.imp_uid;
        msg += '���� �ŷ�ID : ' + rsp.merchant_uid;
        msg += '���� �ݾ� : ' + rsp.paid_amount;
        msg += 'ī�� ���ι�ȣ : ' + rsp.apply_num;
    } else {
        var msg = '������ �����Ͽ����ϴ�.';
        msg += '�������� : ' + rsp.error_msg;
    }
    alert(msg);
});