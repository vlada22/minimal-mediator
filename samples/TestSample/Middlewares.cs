using MinimalMediator.Abstractions.Middleware;
using MinimalMediator.Abstractions.Pipeline;

namespace TestSample
{

    public class ValidationMediatorMiddleware : IPreprocessMiddleware<TestContext>
    {
        private readonly ILogger<ValidationMediatorMiddleware> _logger;
        private readonly ServicesB _servicesB;

        public ValidationMediatorMiddleware(ILogger<ValidationMediatorMiddleware> logger, ServicesB servicesB)
        {
            _logger = logger;
            _servicesB = servicesB;
        }

        public Task InvokeAsync(TestContext context, IPipe<TestContext> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ValidationMiddleware {Context} and {Name}", context, _servicesB.Get());
            return next.InvokeAsync(context, cancellationToken);
        }

        public void Dispose()
        {
            //_logger.LogInformation("ValidationMiddleware disposed");
        }
    }
    
    public class ValidationMiddleware2 : IPreprocessMiddleware<TestContext>
    {
        private readonly ILogger<ValidationMediatorMiddleware> _logger;

        public ValidationMiddleware2(ILogger<ValidationMediatorMiddleware> logger)
        {
            _logger = logger;
        }
        
        public Task InvokeAsync(TestContext context, IPipe<TestContext> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ValidationMiddleware2 {Context}", context);
            return next.InvokeAsync(context, cancellationToken);
        }

        public void Dispose()
        {
            //_logger.LogInformation("ValidationMiddleware2 disposed");
        }
    }
    
    public class ValidationMiddleware3 : IPostProcessMiddleware<TestContext>
    {
        private readonly ILogger<ValidationMediatorMiddleware> _logger;

        public ValidationMiddleware3(ILogger<ValidationMediatorMiddleware> logger)
        {
            _logger = logger;
        }
        
        public Task InvokeAsync(TestContext context, IPipe<TestContext> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ValidationMiddleware3 {Context}", context);
            return next.InvokeAsync(context, cancellationToken);
        }

        public void Dispose()
        {
            //_logger.LogInformation("ValidationMiddleware3 disposed");
        }
    }
    
    public class ExceptionMediatorMiddleware : IPostProcessMiddleware<TestContext>
    {
        private readonly ILogger<ValidationMediatorMiddleware> _logger;

        public ExceptionMediatorMiddleware(ILogger<ValidationMediatorMiddleware> logger)
        {
            _logger = logger;
        }
        
        public Task InvokeAsync(TestContext context, IPipe<TestContext> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("ValidationMiddleware4 {Context}", context);
            return next.InvokeAsync(context, cancellationToken);
        }

        public void Dispose()
        {
            //_logger.LogInformation("ValidationMiddleware4 disposed");
        }
    }
}