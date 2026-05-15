import { createApp } from 'vue';
import PrimeVue from 'primevue/config';
import Aura from '@primeuix/themes/aura';
import Button from 'primevue/button';
import Card from 'primevue/card';
import Divider from 'primevue/divider';
import Tag from 'primevue/tag';
import App from './App.vue';
import './style.css';
import 'primeicons/primeicons.css';

const app = createApp(App);

app.use(PrimeVue, {
  theme: {
    preset: Aura
  }
});

app.component('PButton', Button);
app.component('PCard', Card);
app.component('PDivider', Divider);
app.component('PTag', Tag);

app.mount('#app');
