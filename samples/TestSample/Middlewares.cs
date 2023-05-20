using MinimalMediator.Abstractions.Pipeline;
using MinimalMediator.Core.Context;
using MinimalMediator.Core.Middleware;

namespace TestSample
{

    public class ValidationMediatorMiddleware : IBeforePublishMiddleware<TestContext>
    {
        private readonly ILogger<ValidationMediatorMiddleware> _logger;
        private readonly ServicesB _servicesB;

        public ValidationMediatorMiddleware(ILogger<ValidationMediatorMiddleware> logger, ServicesB servicesB)
        {
            _logger = logger;
            _servicesB = servicesB;
        }

        public Task InvokeAsync(PreProcessMiddlewareContext<TestContext> context,
            IPipe<PreProcessMiddlewareContext<TestContext>, TestContext> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ValidationMiddleware {Context} and {Name}", context.Message, _servicesB.Get());
            return next.InvokeAsync(context, cancellationToken);
        }

        public void Dispose()
        {
            //_logger.LogInformation("ValidationMiddleware disposed");
        }
    }
    
    public class ValidationMediatorMiddleware1 : IBeforePublishMiddleware<TestContext>
    {
        private readonly ILogger<ValidationMediatorMiddleware1> _logger;
        private readonly ServicesB _servicesB;

        public ValidationMediatorMiddleware1(ILogger<ValidationMediatorMiddleware1> logger, ServicesB servicesB)
        {
            _logger = logger;
            _servicesB = servicesB;
        }

        public Task InvokeAsync(PreProcessMiddlewareContext<TestContext> context,
            IPipe<PreProcessMiddlewareContext<TestContext>, TestContext> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ValidationMiddleware1 {Context} and {Name}", context.Message, _servicesB.Get());
            return next.InvokeAsync(context, cancellationToken);
        }

        public void Dispose()
        {
            //_logger.LogInformation("ValidationMiddleware disposed");
        }
    }
    
    public class ValidationMediatorMiddleware2 : IAfterPublishMiddleware<TestContext>
    {
        private readonly ILogger<ValidationMediatorMiddleware2> _logger;
        private readonly ServicesB _servicesB;

        public ValidationMediatorMiddleware2(ILogger<ValidationMediatorMiddleware2> logger, ServicesB servicesB)
        {
            _logger = logger;
            _servicesB = servicesB;
        }

        public Task InvokeAsync(PostProcessMiddlewareContext<TestContext> context,
            IPipe<PostProcessMiddlewareContext<TestContext>, TestContext> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ValidationMiddleware2 {Context} and {Name}", context.Message, _servicesB.Get());
            return next.InvokeAsync(context, cancellationToken);
        }

        public void Dispose()
        {
            //_logger.LogInformation("ValidationMiddleware disposed");
        }
    }
    
    public class ValidationMediatorMiddleware3 : IAfterPublishMiddleware<TestContext>
    {
        private readonly ILogger<ValidationMediatorMiddleware3> _logger;
        private readonly ServicesB _servicesB;

        public ValidationMediatorMiddleware3(ILogger<ValidationMediatorMiddleware3> logger, ServicesB servicesB)
        {
            _logger = logger;
            _servicesB = servicesB;
        }

        public Task InvokeAsync(PostProcessMiddlewareContext<TestContext> context,
            IPipe<PostProcessMiddlewareContext<TestContext>, TestContext> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ValidationMiddleware3 {Context} and {Name}", context.Message, _servicesB.Get());
            return next.InvokeAsync(context, cancellationToken);
        }

        public void Dispose()
        {
            //_logger.LogInformation("ValidationMiddleware disposed");
        }
    }
}