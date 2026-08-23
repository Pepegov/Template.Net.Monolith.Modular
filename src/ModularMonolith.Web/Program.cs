using System.Reflection;
using ModularMonolith.Framework.UseCases.Implementation;
using ModularMonolith.Framework.Utils.Modules;
using ModularMonolith.Module.Builder.Contracts.Implementation;
using ModularMonolith.Module.Template.Contract.Implementation;
using ModularMonolith.Web.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOptions();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.TagActionsBy(api =>
    {
        var tagsAttr = api.ActionDescriptor.EndpointMetadata
            .OfType<TagsAttribute>()
            .FirstOrDefault();
        return tagsAttr is not null
            ? tagsAttr.Tags.ToList()
            : [api.ActionDescriptor.RouteValues["controller"] ?? "Unknown"];
    });
});

builder.Services.RegisterModule<FrameworkModule>(builder.Configuration);
builder.Services.RegisterModule<TemplateModule>(builder.Configuration);
builder.Services.RegisterModule<BuilderModule>(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();                                                                                                                                                                                                                 
app.UseHttpsRedirection();                                                                                                                                                                                                        
app.MapControllers();                                                                                                                                                                                                             

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.RunAsync();