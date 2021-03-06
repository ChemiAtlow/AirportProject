<template>
    <h1 class="airport__title">Welcome To {{ name }} Airport</h1>
    <div class="airport__grid">
        <div class="airport__grid-station__container">
            <router-view v-slot="{ Component }">
                <StationSection>
                    <keep-alive>
                        <component :is="Component" />
                    </keep-alive>
                </StationSection>
            </router-view>
        </div>
        <FlightTables />
    </div>
    <transition name="scroll">
        <ConnectivityBar v-if="connectionState" :state="connectionState" />
    </transition>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { name, hubService } from '@/services';
import FlightTables from '@/views/FlightTables.vue';
import StationSection from '@/views/StationSection.vue';
import ConnectivityBar from '@/components/ConnectivityBar/ConnectivityBar.vue';

const component = defineComponent({
    components: { FlightTables, StationSection, ConnectivityBar },
    setup() {
        return { name, connectionState: hubService.connectionState };
    },
});

export default component;
</script>

<style lang="scss">
* {
    box-sizing: border-box;
}
::-webkit-scrollbar {
    width: 0.4rem;
    &-track {
        background: $secondary;
    }
    &-thumb {
        background: $primary;
        &:hover {
            background: $primaryLight;
        }
    }
}
html,
body {
    margin: 0;
    padding: 0;
    height: 100%;
    background: $grayBg;
    overflow: hidden;
}
#app {
    font-family: Avenir, Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: $primary;
    padding: $pagePadding;
    height: calc(100% - #{$pagePadding});
    .list-enter-active,
    .list-leave-active,
    .list-move {
        transition: all 1s ease;
    }
    .list-enter-from,
    .list-leave-to {
        opacity: 0;
        transform: scale(0.3);
    }

    .scroll-enter-active,
    .scroll-leave-active {
        transition: all 600ms ease;
    }
    .scroll-enter-from,
    .scroll-leave-to {
        transform: translateY(100%);
    }
}
</style>

<style lang="scss" scoped>
.airport {
    &__title {
        display: flex;
        margin: 0;
        height: $titleHeight;
        justify-content: center;
        align-items: center;
    }
    &__grid {
        display: grid;
        grid-template-columns: 1fr 1fr;
        column-gap: 0.5rem;
        height: calc(100% - #{$recordHeight});
        overflow: hidden;
    }
}
</style>
