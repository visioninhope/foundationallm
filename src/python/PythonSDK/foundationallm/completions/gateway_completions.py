class GatewayCompletions:

    def __init__(self, client):
        self.client = client

    def get_completions(self, prompt, max_tokens=100, temperature=0.5, top_p=1.0, frequency_penalty=0.0, presence_penalty=0.0, stop=None, echo=False, logprobs=0, logit_bias=None, return_prompt=False, return_metadata=False, return_logprobs=False, return_tokens=False, return_choices=False, return_sequences=False):
        """Retrieve completions for a given prompt"""
        return self.client.request(
            engine="text-davinci-003",
            prompt=prompt,
            max_tokens=max_tokens,
            temperature=temperature,
            top_p=top_p,
            frequency_penalty=frequency_penalty,
            presence_penalty=presence_penalty,
            stop=stop,
            echo=echo,
            logprobs=logprobs,
            logit_bias=logit_bias,
            return_prompt=return_prompt,
            return_metadata=return_metadata,
            return_logprobs=return_logprobs,
            return_tokens=return_tokens,
            return_choices=return_choices,
            return_sequences=return_sequences
        )
