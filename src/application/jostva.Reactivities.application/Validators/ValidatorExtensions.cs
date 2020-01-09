using FluentValidation;

namespace jostva.Reactivities.application.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            IRuleBuilderOptions<T, string> options = ruleBuilder.NotEmpty()
                                                                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                                                                .Matches("[A-Z]").WithMessage("Password mush contain 1 uppercase letter.")
                                                                .Matches("[a-z]").WithMessage("Password mush have at 1 lowercase character.")
                                                                .Matches("[0-9]").WithMessage("Password mush contain a number.")
                                                                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain non alphanumeric.");

            return options;
        }
    }
}