kiến trúc Onion Architecture
-- cài thư viện
trong core/Application
MediatR.Extensions.Microsoft.DependencyInjection 

trong Presentation/Persistence 
sql, tools

--tham chiếu--
WebApi -> Persistence
Persistence -> Application
Application -> Doamin

Domain/Common  -> khai báo các thành phần dùng chung
Domain/Entity -> tạo ra 1 đối tượng cụ thể ( dùng để tạo bảng trong DB )



# Đăng ký/ Đăng nhập
 - Đăng ký: mỗi tài khoản chỉ được đăng ký 1 email, và để đăng ký cần phải gọi api RequestSendOtp để lấy mã otp gửi về mail (requestType ="sign_upup")
 sau khi có mã otp rồi mới tiến hành đăng ký
 - Đăng nhập chỉ cần nhập nhập email và password

 # user
 - Có các quyền là Admin và client 
 - Chỉ có Admin mới có quyền lấy danh sách user
 - phần quên mật khẩu và thay đổi mật khẩu cũng cần gọi api RequestSendOtp để lấy otp gửi về mail (requestType ="forget_password"|| requestType ="change_password")
sau khi có otp thì gọi api changePassword để đổi mật khẩu (dùng chung cho phần đổi mật khẩu hoặc quên mật khẩu) 
