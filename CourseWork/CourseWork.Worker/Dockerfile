FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /src
RUN apt update
RUN apt install -y protobuf-compiler
COPY . .
RUN cd CourseWork.Worker && dotnet publish -o /app

FROM mcr.microsoft.com/dotnet/runtime:5.0
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT [ "dotnet", "/app/CourseWork.Worker.dll" ]