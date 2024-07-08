FROM ubuntu

RUN apt update
RUN apt --yes install default-jre

ENV PROJECTS_DIR=/home/app/

COPY target/my-own-server-1.0-fat.jar $PROJECTS_DIR

EXPOSE 9090

WORKDIR $PROJECTS_DIR

CMD ["java", "-jar", "my-own-server-1.0-fat.jar"]