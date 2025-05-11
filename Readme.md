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


