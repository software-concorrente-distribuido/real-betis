# Estágio de construção
FROM node:18-alpine as build

ENV PROJECTS_DIR=/home/app/

COPY public $PROJECTS_DIR/public
COPY src $PROJECTS_DIR/src
COPY package-lock.json package.json $PROJECTS_DIR

WORKDIR $PROJECTS_DIR

RUN npm ci
RUN npm run build

# Estágio final
FROM nginx:1.25.3

COPY --from=build /home/app/build /usr/share/nginx/html

# Remove the default NGINX configuration (if any) and copy custom NGINX config
RUN rm /etc/nginx/conf.d/default.conf
COPY nginx/nginx.conf /etc/nginx/conf.d

COPY /etc/letsencrypt/live/realbetis.software/fullchain.pem /etc/nginx/ssl/fullchain.pem
COPY /etc/letsencrypt/live/realbetis.software/privkey.pem /etc/nginx/ssl/privkey.pem

EXPOSE 80 443

CMD ["nginx", "-g", "daemon off;"]
