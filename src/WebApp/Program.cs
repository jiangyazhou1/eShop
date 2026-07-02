using eShop.WebApp.Components;
using eShop.ServiceDefaults;

// 创建 Web 应用程序构建器
var builder = WebApplication.CreateBuilder(args);

// 添加服务默认配置（日志、监控、认证等）
builder.AddServiceDefaults();

// 注册 Razor 组件并启用交互式服务器端组件
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// 注册应用程序自定义服务（购物车、订单等）
builder.AddApplicationServices();

// 构建应用程序实例
var app = builder.Build();

// 映射默认终端点（健康检查、指标等）
app.MapDefaultEndpoints();

// 配置 HTTP 请求管道
// 非开发环境下启用异常处理中间件和 HSTS
if (!app.Environment.IsDevelopment())
{
    // 将未处理的异常重定向到错误页面
    app.UseExceptionHandler("/Error");
    // 启用 HSTS（HTTP 严格传输安全），默认 30 天
    app.UseHsts();
}

// 启用防伪造令牌保护（CSRF 防护）
app.UseAntiforgery();

// 启用 HTTPS 重定向，将所有 HTTP 请求重定向到 HTTPS
app.UseHttpsRedirection();

// 启用静态文件服务（CSS、JavaScript、图片等）
app.UseStaticFiles();

// 映射 Razor 组件路由并启用交互式服务器渲染模式
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

// 配置产品图片代理转发：将 /product-images/{id} 转发到 Catalog API 的图片端点
app.MapForwarder("/product-images/{id}", "https+http://catalog-api", "/api/catalog/items/{id}/pic");

// 启动应用程序并开始监听请求
app.Run();
