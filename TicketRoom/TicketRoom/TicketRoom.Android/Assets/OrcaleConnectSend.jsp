<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd"> 
<html> 
<head> 
<meta http-equiv="Content-Type" content="text/html; charset=EUC-KR"> 
<title>DB로 데이터 보내기</title> 
</head> 
<body> 
<% 
      try { 
           String driver="oracle.jdbc.driver.OracleDriver"; 
           Class.forName(driver); 
           String url=" jdbc:oracle:thin:@175.115.110.17:1521:ORCL" ; 
           String userName="c##twoh" ; 
           String passWord="Twohadmin120" ; 
           Connection con = DriverManager.getConnection(url,userName,passWord); 
           Statement st = con.createStatement(); 
           //데이터 입력받아서 sql에 넣기 
           String sql="insert into GENERAL(GNAME , GAGE , GMONEY) values(
            ' "+"aaa"+" ',"+ "bbb" +","+ "ccc" +" )"; 
            st.executeUpdate(sql); 
            con.close(); 
            st.close(); 

       } catch (Exception e) { 
                   System.out.println(e);	
       } 

%> 

</body> 
</html>