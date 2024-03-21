FROM python:3.11-slim
ARG FOUNDATIONALLM_VERSION
ENV FOUNDATIONALLM_VERSION=${FOUNDATIONALLM_VERSION:-0.0.0}
ENV APPLICATIONINSIGHTS_STATSBEAT_DISABLED_ALL=true

RUN apt-get update \
        && apt-get install -y curl apt-transport-https gnupg2 \
        && curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
        && curl https://packages.microsoft.com/config/debian/11/prod.list > /etc/apt/sources.list.d/mssql-release.list \
        && apt-get update \
        && ACCEPT_EULA=Y apt-get install -y msodbcsql18 mssql-tools

# RUN apk add curl gnupg

# #Download the desired package(s)
# RUN curl -O https://download.microsoft.com/download/b/9/f/b9f3cce4-3925-46d4-9f46-da08869c6486/msodbcsql18_18.0.1.1-1_amd64.apk \
# 	&& curl -O https://download.microsoft.com/download/b/9/f/b9f3cce4-3925-46d4-9f46-da08869c6486/mssql-tools18_18.0.1.1-1_amd64.apk

# #(Optional) Verify signature, if 'gpg' is missing install it using 'apk add gnupg':
# RUN curl -O https://download.microsoft.com/download/b/9/f/b9f3cce4-3925-46d4-9f46-da08869c6486/msodbcsql18_18.0.1.1-1_amd64.sig \
# 	&& curl -O https://download.microsoft.com/download/b/9/f/b9f3cce4-3925-46d4-9f46-da08869c6486/mssql-tools18_18.0.1.1-1_amd64.sig

# RUN curl https://packages.microsoft.com/keys/microsoft.asc  | gpg --import - \
# 	&& gpg --verify msodbcsql18_18.0.1.1-1_amd64.sig msodbcsql18_18.0.1.1-1_amd64.apk \
# 	&& gpg --verify mssql-tools18_18.0.1.1-1_amd64.sig mssql-tools18_18.0.1.1-1_amd64.apk


# #Install the package(s)
# RUN apk add --allow-untrusted msodbcsql18_18.0.1.1-1_amd64.apk \
# 	&& apk add --allow-untrusted mssql-tools18_18.0.1.1-1_amd64.apk \
# 	&& rm -f msodbcsql18_18.0.1.1-1_amd64.apk mssql-tools18_18.0.1.1-1_amd64.apk

WORKDIR /code

# RUN apk add --no-cache postgresql-libs 
# RUN apk add --no-cache --virtual .build-deps gcc musl-dev postgresql-dev 
RUN apt-get update \
    && apt-get -y install libpq-dev gcc \
    && pip install psycopg2
COPY ./LangChainAPI/requirements.txt /code/requirements.txt
RUN python3 -m pip install --no-cache-dir --upgrade torch --index-url https://download.pytorch.org/whl/cpu 
RUN python3 -m pip install --no-cache-dir --upgrade -r /code/requirements.txt

# RUN apk --purge del .build-deps

COPY ./PythonSDK/foundationallm /code/foundationallm
COPY ./LangChainAPI /code

EXPOSE 80

CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "80"]
LABEL org.opencontainers.image.source=https://github.com/solliancenet/foundationallm
LABEL ai.foundationallm.version=${FOUNDATIONALLM_VERSION:-0.0.0}
