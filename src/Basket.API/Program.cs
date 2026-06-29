// 创建 Web 应用程序构建器并配置应用程序
var builder = WebApplication.CreateBuilder(args);

// 注册基础服务默认配置（日志、监控等）
builder.AddBasicServiceDefaults();
// 注册应用程序服务（Redis、事件总线等）
builder.AddApplicationServices();

// 注册 gRPC 服务支持
builder.Services.AddGrpc();

// 构建应用程序
var app = builder.Build();

// 映射默认端点（健康检查等）
app.MapDefaultEndpoints();

// 注册购物车 gRPC 服务
app.MapGrpcService<BasketService>();

// 启动应用程序
app.Run();
