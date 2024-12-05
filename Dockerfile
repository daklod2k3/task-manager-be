FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV Authentication:JwtSecret="79Gv63UuIofChCmGRUGbPtuJiIUG+cMj7ZhC/xpA1dcFEFB9zSLC4W9GaFGOIMjG3WR2xkBhPytCr9XtYj0Jcw=="
ENV ConnectionStrings:DefaultConnection="Host=aws-0-ap-southeast-1.pooler.supabase.com;Database=postgres;Username=postgres.ndoyladxdcpftovoalas;Password=MCuqRZ7OpgayaN4d;Port=5432;Include Error Detail=true;"
ENV SUPABASE_PUB_KEY="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im5kb3lsYWR4ZGNwZnRvdm9hbGFzIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MjY0NzExNTcsImV4cCI6MjA0MjA0NzE1N30.gF1Kn0no1-uVQ-FIskQKHJ60XUl9yuQ4dUcOjiBu_WE"
ENV SUPABASE_KEY="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im5kb3lsYWR4ZGNwZnRvdm9hbGFzIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTcyNjQ3MTE1NywiZXhwIjoyMDQyMDQ3MTU3fQ.wc4QVvO4Q0iB-rh1gu6SfdOps69w6UR-3NrqaKB1C1c"
ENV SUPABASE_URL="https://ndoyladxdcpftovoalas.supabase.co"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["server/server.csproj", "server/"]
RUN dotnet restore "server/server.csproj"
COPY . .
WORKDIR "/src/server"
RUN dotnet build "server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "server.dll"]
